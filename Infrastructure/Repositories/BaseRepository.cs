using DesafioBackendAPI.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackendAPI.Infrastructure.Repositories
{
    public class BaseRepository : IBaseRepository
    {
        private readonly Contexto _db;

        public BaseRepository(Contexto db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public async Task<bool> GetSituacaoAsync(int contaID)
        {
            var conta = await _db.Conta.FirstOrDefaultAsync(d => d.Id == contaID).ConfigureAwait(false);

            if (conta == null)
                throw new Exception("Conta inexistente.");

            var situacao = conta.DtExclusao == null && conta.Situacao == "Ativa";

            return situacao;
        }

        public async Task<decimal> GetSaldoAsync(int id)
        {
            var conta = await _db.Conta.FirstOrDefaultAsync(s => s.Id == id).ConfigureAwait(false);
            if (conta == null)
                throw new Exception("Conta inexistente.");

            _db.Entry(conta).State = EntityState.Detached;

            return conta.Saldo;
        }

        public async Task<DateTime> GetDtInclusaoAsync(int id)
        {
            var conta = await _db.Conta.FirstOrDefaultAsync(s => s.Id == id).ConfigureAwait(false);

            if (conta == null)
                throw new Exception("Conta inexistente.");

            var dataInclusao = conta.DtInclusao;

            _db.Entry(conta).State = EntityState.Detached;

            return conta.DtInclusao;
        }
    }
}
