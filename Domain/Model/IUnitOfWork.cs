using System.Data;

namespace DesafioBackendAPI.Domain.Model
{
    public interface IUnitOfWork
    {
        Task SaveChangesAsync();

        IDbTransaction BeginTransaction();
    }
}