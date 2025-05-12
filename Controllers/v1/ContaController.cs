using Asp.Versioning;
using AutoMapper;
using DesafioBackendAPI.Application.DTOs;
using DesafioBackendAPI.Application.Services;
using DesafioBackendAPI.Domain.Model;
using DesafioBackendAPI.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

namespace DesafioBackendAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/Conta")]
    [ApiVersion("1.0")]
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

        #region CRUD

        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> Add(ContaDTO contadto)
        {
            try
            {
                _logger.LogInformation("Adicionando registro...");

                if (contadto == null)
                    return BadRequest("Dados não podem ser nulos.");

                if (!FormataString.SomenteNumeros(contadto.Cnpj))
                    return BadRequest("CNPJ deve conter apenas números.");

                if (contadto.Cnpj.Length != 14)
                    return BadRequest("CNPJ deve conter 14 números.");

                var _conta = await _db.Conta.FirstOrDefaultAsync(c => c.Agencia.Trim().Equals(contadto.Agencia.Trim()) && c.Numero.Trim().Equals(contadto.Numero.Trim())).ConfigureAwait(false);
                if (_conta != null)
                    return BadRequest("Conta existente.");

                if (RetornaReceita.RetornaNome(contadto.Cnpj).Result == null)
                    return BadRequest("CNPJ inválido.");

                var conta_agencia_numero = "_" + contadto.Numero + contadto.Agencia;
                var complemento_nome = RandomNumberGenerator.GetInt32(100000000, int.MaxValue);
                var local_arquivo = Path.Combine("Armazenamento", conta_agencia_numero + complemento_nome + System.IO.Path.GetExtension(contadto.Arquivo.FileName));

                var conta = _mapper.Map<Conta>(contadto);

                //Campos não mapeados
                conta.LocalArquivo = local_arquivo;
                conta.DtInclusao = DateTime.Now;
                conta.Id = null;

                await Task.Run(() => _repository.AddAsync(conta)); //Adiciona o registro DTO convertido no modelo, no banco de dados.

                await RetornaArquivo.SalvarArquivoDisco(conta_agencia_numero, local_arquivo, contadto);

                var apagar_arquivos = Directory.GetFiles("Armazenamento", "conta_agencia_numero*");
                foreach (var file in apagar_arquivos)
                {
                    System.IO.File.Delete(file);
                }
                using (Stream fileStream = new FileStream(local_arquivo, FileMode.Create))
                {
                    await contadto.Arquivo.CopyToAsync(fileStream).ConfigureAwait(false);
                }

                _logger.LogInformation("Registro id: " + conta.Id + " adicionado!");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("Buscando registros...");

                var contas = await Task.Run(() => _repository.GetAsync()).ConfigureAwait(false);

                if (!contas.Any())
                    return NotFound("Lista de contas é nula ou vazia.");

                foreach (var conta in contas)
                {
                    var nome = RetornaReceita.RetornaNome(conta.Cnpj).Result;

                    if (nome == "TooManyRequests")
                        conta.NomeCompleto = "Número de requisições acima do limite permitido em ReceitaWS (HTTP 429).";
                    else if (nome != null)
                        conta.NomeCompleto = nome;
                    else
                        conta.NomeCompleto = "Ocorreu um erro na solicitação.";
                }

                var contasDTO = _mapper.Map<IEnumerable<ContaDTO>>(contas);

                _logger.LogInformation("Registros retornados.");

                return Ok(contasDTO); //Retorna solicitação com DTOs
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get(int Id)
        {
            try
            {
                _logger.LogInformation("Buscando registro...");

                var _conta = await Task.Run(() => _repository.GetAsync(Id));

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

        [HttpGet("Download/{nome}")]
        public async Task<IActionResult> Download(string nome)
        {
            try
            {
                _logger.LogInformation("Buscando arquivo por nome: " + nome);

                var arquivo_nome = nome;
                var local_arquivo = Path.Combine(Directory.GetCurrentDirectory(), "Armazenamento");
                var local_arquivo_completo = Path.Combine(local_arquivo, arquivo_nome);

                if (!System.IO.File.Exists(local_arquivo_completo))
                {
                    return BadRequest("Arquivo informado inexistente");
                }

                var fileBytes = await RetornaArquivo.RecuperarArquivoDisco(local_arquivo_completo).ConfigureAwait(false);

                _logger.LogInformation("Download do arquivo foi efetuado");

                return File(fileBytes, "application/octet-stream", arquivo_nome);
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(int Id, ContaDTO contadto)
        {
            try
            {
                _logger.LogInformation("Adicionando registro...");

                if (contadto == null)
                    return BadRequest("conta não pode ser nulo.");

                if (!FormataString.SomenteNumeros(contadto.Cnpj))
                    return BadRequest("CNPJ deve conter apenas números.");

                if (contadto.Cnpj.Length != 14)
                    return BadRequest("CNPJ deve conter 14 números.");

                var situacao = await Task.Run(() => _baseRepository.GetSituacaoAsync(contadto.Id.Value)).ConfigureAwait(false);
                if (!situacao)
                    return BadRequest("Conta inativa e/ou excluída!");

                if (RetornaReceita.RetornaNome(contadto.Cnpj).Result == null)
                    return BadRequest("CNPJ inválido.");

                var DtInclusao = await _baseRepository.GetDtInclusaoAsync(Id).ConfigureAwait(false);

                var conta_agencia_numero = "_" + contadto.Numero + contadto.Agencia;
                var complemento_nome = RandomNumberGenerator.GetInt32(100000000, int.MaxValue);
                var local_arquivo = Path.Combine("Armazenamento", conta_agencia_numero + complemento_nome + System.IO.Path.GetExtension(contadto.Arquivo.FileName));

                var _conta = _mapper.Map<Conta>(contadto);

                //Campos não mapeados
                _conta.DtInclusao = DtInclusao;
                _conta.DtAlteracao = DateTime.Now;
                _conta.LocalArquivo = local_arquivo;
                _conta.Saldo = await Task.Run(() => _baseRepository.GetSaldoAsync(contadto.Id.Value)).ConfigureAwait(false);


                //Adiciona o registro DTO convertido no modelo, no banco de dados.
                await Task.Run(() => _repository.UpdateAsync(_conta)).ConfigureAwait(false);

                //Salvar arqquivo em disco só depois de salvar registro no banco de dados.
                RetornaArquivo.SalvarArquivoDisco(conta_agencia_numero, local_arquivo, contadto);

                _logger.LogInformation("Registro id: " + _conta.Id + " alterado!");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

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

                if (conta.DtExclusao != null)
                    return BadRequest("Não é possível excluir uma conta que já foi excluída.");

                await _repository.DeleteAsync(id).ConfigureAwait(false); // Fix for CRR0029

                _logger.LogInformation("Registro id: " + id + " excluído!");

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        #endregion CRUD       

    }
}