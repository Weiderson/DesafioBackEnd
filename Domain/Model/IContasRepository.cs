namespace DesafioBackendAPI.Domain.Model
{
    public interface IContaRepository
    {
        Task AddAsync(Conta conta);

        Task<IEnumerable<Conta>> GetAsync();

        Task<Conta> GetAsync(int id);

        Task DeleteAsync(int id);

        Task UpdateAsync(Conta conta);

    }
}