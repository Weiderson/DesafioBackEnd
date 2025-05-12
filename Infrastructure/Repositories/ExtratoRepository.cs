using DesafioBackendAPI.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackendAPI.Infrastructure.Repositories
{
    public class ExtratoRepository : IExtratroRepository
    {
        private readonly Contexto _db;

        public ExtratoRepository(Contexto db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        #region EXTRATO

        public async Task<IEnumerable<Transacao>> GetExtratoAsync(int id, int periodo)
        {

            var extrato = await _db.Transacao.Where(w => w.ContaId == id && w.DtInclusao >= DateTime.Now.AddDays(-periodo) || w.ContaId_Destino == id).OrderByDescending(d => d.DtInclusao).ToListAsync().ConfigureAwait(false);

            return extrato ?? Enumerable.Empty<Transacao>();
        }

        #endregion

    }
}