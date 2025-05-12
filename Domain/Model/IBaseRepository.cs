namespace DesafioBackendAPI.Domain.Model
{
    public interface IBaseRepository
    {
        Task<bool> GetSituacaoAsync(int contaId);

        Task<Decimal> GetSaldoAsync(int contaId);

        Task<DateTime> GetDtInclusaoAsync(int contaId);
    }
}
