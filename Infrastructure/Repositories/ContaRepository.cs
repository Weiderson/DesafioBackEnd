using DesafioBackendAPI.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackendAPI.Infrastructure.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly Contexto _db;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public ContaRepository(Contexto db, IHttpContextAccessor httpContextAccessor)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        #region CRUD

        public async Task AddAsync(Conta conta)
        {
            _db.Conta.Add(conta);
            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Conta>> GetAsync()
        {
            var contas = await _db.Conta.ToListAsync().ConfigureAwait(false);
            return contas;
        }

        public async Task<Conta> GetAsync(int id)
        {
            var conta = await _db.Conta.FirstOrDefaultAsync(f => f.Id == id);
            return conta;
        }

        public async Task DeleteAsync(int id)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            string caminho = httpContext?.Request.Path.ToString();

            if (caminho == null)
                throw new Exception("Caminho não encontrado.");


            var conta = await GetAsync(id).ConfigureAwait(false);
            if (conta is null)
                throw new Exception("Conta inexistente.");

            if (caminho.StartsWith("/api/v1"))
            {
                conta.DtExclusao = DateTime.Now;
                _db.Conta.Update(conta);
            }
            else if (caminho.StartsWith("/api/v2"))
            {
                conta.DtExclusao = DateTime.Now;
                _db.Conta.Remove(conta);
            }

            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(Conta conta)
        {
            _db.Conta.Update(conta);

            await _db.SaveChangesAsync().ConfigureAwait(false);
        }

        #endregion CRUD
    }
}