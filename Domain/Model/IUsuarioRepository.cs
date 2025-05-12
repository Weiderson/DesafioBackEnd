namespace DesafioBackendAPI.Domain.Model
{
    public interface IUsuarioRepository
    {
        Task<IEnumerable<Usuario>> GetAsync();

        Task<Usuario> GetAsync(string email);

        Task AddAsync(Usuario usuario);

        Task UpdateAsync(Usuario usuario);

        Task DeleteAsync(Usuario usuario);

        Task<Usuario> UsuarioExisteAsync(string email);

        Task<Usuario> UsuarioExisteExcluidoAsync(string email);

    }
}
