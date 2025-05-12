using AutoMapper;
using DesafioBackendAPI.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackendAPI.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly Contexto _db;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuarioRepository(Contexto db, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _db = db;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Usuario> GetAsync(string email)
        {
            var usuario = await UsuarioExisteAsync(email).ConfigureAwait(false);
            return usuario;
        }
        public async Task<IEnumerable<Usuario>> GetAsync()
        {
            var usuarios = await _db.Usuario.Where(u => u.DtExclusao == null).ToListAsync().ConfigureAwait(false);
            return usuarios;
        }

        public async Task AddAsync(Usuario usuario)
        {
            _db.Usuario.Add(usuario);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _db.Update(usuario);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteAsync(Usuario usuario)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            string? caminho = httpContext?.Request.Path.ToString();

            if (!string.IsNullOrEmpty(caminho) && caminho.StartsWith("/api/v1/"))
            {
                usuario.DtExclusao = DateTime.Now;
                _db.Usuario.Update(usuario);
            }
            else if (!string.IsNullOrEmpty(caminho) && caminho.StartsWith("/api/v2/"))
            {
                usuario.DtExclusao = DateTime.Now;
                _db.Usuario.Remove(usuario);
            }

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<Usuario> UsuarioExisteAsync(string email)
        {
            var usuario = await _db.Usuario.FirstOrDefaultAsync(u => u.Email == email).ConfigureAwait(false);
            return usuario;
        }

        public async Task<Usuario> UsuarioExisteExcluidoAsync(string email)
        {
            var usuario = await _db.Usuario.FirstOrDefaultAsync(u => u.Email == email && u.DtExclusao == null).ConfigureAwait(false);
            return usuario;
        }
    }

}