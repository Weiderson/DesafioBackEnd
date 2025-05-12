namespace DesafioBackendAPI.Domain.Model
{
    public interface IExtratroRepository
    {
        Task<IEnumerable<Transacao>> GetExtratoAsync(int id, int periodo);
    }
}