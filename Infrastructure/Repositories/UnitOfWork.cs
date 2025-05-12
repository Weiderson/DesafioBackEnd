using DesafioBackendAPI.Domain.Model;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace DesafioBackendAPI.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Contexto _db;

        public UnitOfWork(Contexto db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
        }

        public IDbTransaction BeginTransaction()
        {
            var transaction = _db.Database.BeginTransaction();
            return transaction.GetDbTransaction();
        }

        public Task SaveChangesAsync()
        {
            return _db.SaveChangesAsync();
        }

    }
}
