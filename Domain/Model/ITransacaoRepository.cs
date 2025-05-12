using DesafioBackendAPI.Domain.Model;

namespace DesafioBackendAPI.Infrastructure.Repositories
{
    public interface ITransacaoRepository
    {
        Task AddAsync(Transacao transacao);

        Task<IEnumerable<Transacao>> GetAsync();

        Task<IEnumerable<Transacao>> GetTransacaoAsync(int contaid);

        Task UpdateSaldoAsync(int contaId, decimal saldo_atualizado);

        Task UpdateContaDestinoAsync(int ContaId_Destino, decimal valor);
    }
}