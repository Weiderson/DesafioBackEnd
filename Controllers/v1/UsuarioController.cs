using Asp.Versioning;
using AutoMapper;
using DesafioBackendAPI.Application.DTOs;
using DesafioBackendAPI.Application.Services.Token;
using DesafioBackendAPI.Domain.Model;
using DesafioBackendAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace DesafioBackendAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/Usuario")]
    [ApiVersion("1.0")]
    public class UsuarioController : ControllerBase
    {
        private readonly Contexto _db;
        private readonly IUsuarioRepository _repository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(Contexto db, IUsuarioRepository repository, IMapper mapper, IConfiguration configuration, ILogger<UsuarioController> logger)
        {
            _db = db;
            _repository = repository;
            _mapper = mapper;
            _configuration = configuration;
            _tokenService = new TokenService(configuration);
            _logger = logger;
        }

        #region CRUD

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("Buscando registros...");

                var usuarios = (await _repository.GetAsync().ConfigureAwait(false)).ToList();

                if (!usuarios.Any())
                    return NotFound("Lista de usuários é nula ou vazia.");

                var usuariosDTO = _mapper.Map<IEnumerable<UsuarioDTO>>(usuarios);

                _logger.LogInformation("Registros retornados...");
                return Ok(usuariosDTO);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{email}")]
        public async Task<IActionResult> Get(string email)
        {
            try
            {
                _logger.LogInformation("Buscando registro...");

                var usuario = await _repository.GetAsync(email).ConfigureAwait(false);
                if (usuario == null)
                    return NotFound("Usuário não encontrado.");

                var usuarioDTO = _mapper.Map<UsuarioDTO>(usuario);

                _logger.LogInformation("Registro retornado...");
                return Ok(usuarioDTO);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromQuery] UsuarioDTO usuarioDTO)
        {
            try
            {
                _logger.LogInformation("Adicionando registro...");

                if (usuarioDTO == null)
                    return BadRequest("Dados inválidos.");

                if (usuarioDTO.Email == null)
                    return BadRequest("Email inválido.");

                if (usuarioDTO.Senha == null)
                    return BadRequest("Senha inválida.");

                if (usuarioDTO.Nome == null)
                    return BadRequest("Nome inválido.");


                var usuarioExiste = _repository.UsuarioExisteAsync(usuarioDTO.Email);
                if (usuarioExiste != null)
                    return BadRequest("Usuário já existe.");

                var usuario = _mapper.Map<Usuario>(usuarioDTO);
                usuario.Id = null;
                usuario.DtInclusao = DateTime.Now;

                using (var hmac = new HMACSHA256())
                {
                    usuario.SenhaHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(usuarioDTO.Senha));
                    usuario.SenhaSalt = hmac.Key;
                }

                await Task.Run(() => _repository.AddAsync(usuario));
                _logger.LogInformation("Registro adicionado...");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] UsuarioDTO usuarioDTO)
        {
            try
            {
                _logger.LogInformation("Atualizando registro...");

                if (usuarioDTO == null)
                    return BadRequest("Dados inválidos.");
                if (usuarioDTO.Email == null)
                    return BadRequest("Email inválido.");
                if (usuarioDTO.Senha == null)
                    return BadRequest("Senha inválida.");
                if (usuarioDTO.Nome == null)
                    return BadRequest("Nome inválido.");

                var usuario = await _repository.GetAsync(usuarioDTO.Email);
                if (usuario == null)
                    return NotFound("Usuário não encontrado.");

                _mapper.Map<Usuario>(usuarioDTO);
                usuario.DtAlteracao = DateTime.Now;

                using (var hmac = new HMACSHA256())
                {
                    usuario.SenhaHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(usuarioDTO.Senha));
                    usuario.SenhaSalt = hmac.Key;
                }

                await Task.Run(() => _repository.UpdateAsync(usuario)).ConfigureAwait(false);
                _logger.LogInformation("Registro atualizado...");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{email}")]
        public async Task<IActionResult> Delete(string email)
        {
            try
            {
                _logger.LogInformation("Excluindo registro");
                var usuario = await _repository.GetAsync(email);
                if (usuario == null)
                    return NotFound("Usuário não encontrado.");

                await Task.Run(() => _repository.DeleteAsync(usuario)).ConfigureAwait(false);
                _logger.LogInformation("Registro excluído.");
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromQuery] UsuarioDTO usuarioDTO)
        {
            try
            {
                _logger.LogInformation("Efetuando login");
                if (usuarioDTO == null)
                    return BadRequest("Dados inválidos.");

                if (usuarioDTO.Email == null)
                    return BadRequest("Email inválido.");

                if (usuarioDTO.Senha == null)
                    return BadRequest("Senha inválida.");

                var usuario = await _repository.UsuarioExisteExcluidoAsync(usuarioDTO.Email).ConfigureAwait(false);
                if (usuario == null)
                    return BadRequest("Usuário inexistente ou excluído.");

                var hash = new HMACSHA256(usuario.SenhaSalt);
                var result = hash.ComputeHash(Encoding.UTF8.GetBytes(usuarioDTO.Senha));

                if (!usuario.SenhaHash.SequenceEqual(result))
                    return BadRequest("Senha inválida.");

                var token = _tokenService.CriarToken(usuario);

                hash.Dispose();
                _logger.LogInformation("Login efetuado");
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        #endregion
    }
}
