using Asp.Versioning;
using AutoMapper;
using DesafioBackendAPI.Application.DTOs;
using DesafioBackendAPI.Application.Services;
using DesafioBackendAPI.Domain.Model;
using DesafioBackendAPI.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioBackendAPI.Controllers.v2
{
    [ApiController]
    [Route("api/v{version:apiVersion}/Conta")]
    [ApiVersion("2.0")]
    public class ContaController : ControllerBase
    {
        private readonly Contexto _db;
        private readonly IContaRepository _repository;
        private readonly ILogger<ContaController> _logger;
        private readonly IMapper _mapper;
        private readonly IBaseRepository _baseRepository;

        public ContaController(Contexto db, ILogger<ContaController> logger, IContaRepository repository, IMapper mapper, IBaseRepository baseRepository)
        {
            _db = db;
            _repository = repository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper;
            _baseRepository = baseRepository;
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            try
            {
                _logger.LogInformation("Buscando registro...");

                var _conta = await Task.Run(() => _repository.GetAsync(Id)).ConfigureAwait(false);

                if (_conta == null)
                    return NotFound("Conta inexistente.");

                var nome = RetornaReceita.RetornaNome(_conta.Cnpj).Result;

                if (nome == "TooManyRequests")
                    _conta.NomeCompleto = "Número de requisições acima do limite permitido em ReceitaWS (HTTP 429).";
                else if (nome != null)
                    _conta.NomeCompleto = nome;
                else
                    _conta.NomeCompleto = "Ocorreu um erro na solicitação.";

                var _contaDTO = _mapper.Map<ContaDTO>(_conta);

                _logger.LogInformation("Registro id: " + Id + "retornado.");

                return Ok(_contaDTO); //Retorna solicitação com DTOs
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        #region EXCLUSÃO FÍSICA

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Deletando registro id: " + id);

                var conta = await _db.Conta.FindAsync(id).ConfigureAwait(false);

                if (conta == null)
                    return NotFound("Registro não encontrado.");

                if (conta.Id.HasValue)
                {
                    var saldo = await _baseRepository.GetSaldoAsync(conta.Id.Value).ConfigureAwait(false);

                    if (saldo != 0)
                        return BadRequest("Não é possível excluir uma conta com saldo diferente de zero.");
                }

                await _repository.DeleteAsync(id).ConfigureAwait(false);

                _logger.LogInformation("Registro id: " + id + " excluído!");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        #endregion CRUD       
    }
}