using Asp.Versioning;
using AutoMapper;
using DesafioBackendAPI.Application.DTOs;
using DesafioBackendAPI.Application.Enums;
using DesafioBackendAPI.Domain.Model;
using DesafioBackendAPI.Infrastructure;
using DesafioBackendAPI.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DesafioBackendAPI.Controllers.v1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/Transacao")]
    [ApiVersion("1.0")]
    public class TransacaoController : ControllerBase
    {
        private readonly Contexto _db;
        private readonly ITransacaoRepository _repository;
        private readonly ILogger<TransacaoController> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _transaction;
        private readonly IBaseRepository _baseRepository;

        public TransacaoController(Contexto db, ITransacaoRepository repository, ILogger<TransacaoController> logger, IMapper mapper, IUnitOfWork transaction, IBaseRepository baseRepository)
        {
            _db = db;
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _transaction = transaction;
            _baseRepository = baseRepository;
        }

        #region TRANSAÇÕES

        [HttpPost]
        public async Task<IActionResult> Add([FromQuery] TransacaoDTO transacao_dto)
        {
            using (var transaction = _transaction.BeginTransaction())
            {
                try
                {
                    _logger.LogInformation("Adicionando registro...");

                    if (transacao_dto == null)
                        return BadRequest("Dados não podem ser nulos.");

                    if (!Enum.IsDefined(typeof(TransacaoTipo), transacao_dto.Tipo))
                        return BadRequest("Tipo de transação inválido.");

                    // VERIFICAÇÃO INICIAL - CONTA
                    var situacao = await Task.Run(() => _baseRepository.GetSituacaoAsync(transacao_dto.ContaId));

                    if (!situacao)
                        return BadRequest("Conta inativa e/ou excluída!");

                    // PAGAMENTO - SAQUE
                    if (transacao_dto.Tipo == TransacaoTipo.Pagamento || transacao_dto.Tipo == TransacaoTipo.Saque)
                    {
                        if (transacao_dto.Valor <= 0)
                            return BadRequest("Valor deve ser maior que zero.");

                        transacao_dto.ContaId_Destino = null;

                        var saldo = await Task.Run(() => _baseRepository.GetSaldoAsync(transacao_dto.ContaId)).ConfigureAwait(false);
                        if (saldo < transacao_dto.Valor)
                            return BadRequest("Saldo insuficiente para realizar a transação.");

                        var saldo_atualizado = saldo - transacao_dto.Valor;

                        transacao_dto.ContaId_Destino = null;

                        await Task.Run(() => _repository.UpdateSaldoAsync(transacao_dto.ContaId, saldo_atualizado));

                        await _transaction.SaveChangesAsync();
                    }

                    //TRANSFERÊNCIA
                    else if (transacao_dto.Tipo == TransacaoTipo.Transferencia)
                    {
                        try
                        {
                            if (transacao_dto.Valor <= 0)
                                return BadRequest("Valor deve ser maior que zero.");

                            if (transacao_dto.ContaId_Destino == null)
                                return BadRequest("Conta de destino inválida.");

                            if (transacao_dto.ContaId == transacao_dto.ContaId_Destino)
                                return BadRequest("Conta de origem e destino são iguais.");

                            if (string.IsNullOrEmpty(transacao_dto.ContaId_Destino.ToString()))
                                return BadRequest("Agência e conta de destino devem ser informadas.");

                            var saldo = await Task.Run(() => _baseRepository.GetSaldoAsync(transacao_dto.ContaId)).ConfigureAwait(false);
                            if (saldo < transacao_dto.Valor)
                                return BadRequest("Saldo insuficiente para realizar a transação.");

                            var saldo_atualizado = saldo - transacao_dto.Valor;


                            var situacao_conta_destino = await Task.Run(() => _baseRepository.GetSituacaoAsync(transacao_dto.ContaId_Destino ?? 0)).ConfigureAwait(false);

                            if (!situacao_conta_destino)
                                return BadRequest("Conta de destino inativa e/ou excluída!");


                            await Task.Run(() => _repository.UpdateContaDestinoAsync(transacao_dto.ContaId_Destino ?? 0, transacao_dto.Valor))
                                .ConfigureAwait(false);
                            await _transaction.SaveChangesAsync().ConfigureAwait(false);

                            await Task.Run(() => _repository.UpdateSaldoAsync(transacao_dto.ContaId, saldo_atualizado)).ConfigureAwait(false);
                            await _transaction.SaveChangesAsync().ConfigureAwait(false);


                        }
                        catch (Exception ex)
                        {
                            _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                            return BadRequest(ex.Message);
                        }
                    }

                    // DEPÓSITO
                    else if (transacao_dto.Tipo == TransacaoTipo.Deposito)
                    {
                        if (transacao_dto.Valor <= 0)
                            return BadRequest("Valor deve ser maior que zero.");

                        transacao_dto.ContaId_Destino = null;

                        var saldo = await Task.Run(() => _baseRepository.GetSaldoAsync(transacao_dto.ContaId));

                        var saldo_atualizado = saldo + transacao_dto.Valor;

                        transacao_dto.ContaId_Destino = null;

                        await Task.Run(() => _repository.UpdateSaldoAsync(transacao_dto.ContaId, saldo_atualizado)).ConfigureAwait(false);

                        await _transaction.SaveChangesAsync().ConfigureAwait(false);
                    }


                    var _transacao = _mapper.Map<Transacao>(transacao_dto);
                    _transacao.DtInclusao = DateTime.Now;
                    _transacao.Id = null;
                    _transacao.Descricao.ToUpper();

                    // ADICIONA A TRANSAÇÃO DA CONTA DE ORIGEM
                    await Task.Run(() => _repository.AddAsync(_transacao)).ConfigureAwait(false);
                    await _transaction.SaveChangesAsync().ConfigureAwait(false);

                    //throw new Exception("Erro de teste!");

                    transaction.Commit();

                    _logger.LogInformation("Transação id: " + _transacao.Id + " adicionada!");

                    return Ok();
                }
                catch (Exception ex)
                {
                    _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);

                    transaction.Rollback();

                    return BadRequest(ex.Message);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                _logger.LogInformation("Buscando registros...");

                var _transacoes = await Task.Run(() => _repository.GetAsync()).ConfigureAwait(false);

                if (!_transacoes.Any())
                    return NotFound("Lista de transacões nula ou vazia.");

                var _transacoesDTO = _mapper.Map<IEnumerable<TransacaoDTO>>(_transacoes);

                _logger.LogInformation("Registros retornados.");

                return Ok(_transacoesDTO); //Retorna solicitação com DTOs
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                _logger.LogInformation("Buscando registro...");

                var situacao = await Task.Run(() => _baseRepository.GetSituacaoAsync(id)).ConfigureAwait(false);
                if (!situacao)
                    return BadRequest("Conta inativa e/ou excluída!");

                var _transacao = await _repository.GetTransacaoAsync(id).ConfigureAwait(false);

                if (!_transacao.Any())
                    return NotFound("Lista de transacões nula ou vazia.");

                var _transacoesDTO = _mapper.Map<IEnumerable<TransacaoDTO>>(_transacao);

                _logger.LogInformation("Registro retornado.");

                return Ok(_transacoesDTO); //Retorna solicitação com DTOs
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, "Ocorreu um erro na requisição: " + ex.Message);
                return NotFound(ex.Message);
            }
        }

        #endregion TRANSAÇÕEES
    }
}