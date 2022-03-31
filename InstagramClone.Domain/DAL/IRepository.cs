using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sieve.Services;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Domain.DAL
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Insert(TEntity entity);

        Task InsertAsync(TEntity entity, CancellationToken token);

        EntityEntry<TEntity> GetEntityEntry(TEntity entity);

        TEntity Find(params object[] keys);

        ValueTask<TEntity> FindAsync(CancellationToken token, params object[] keys);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        DbSet<TEntity> Query { get; }

        Task<int> SaveChangesAsync(CancellationToken token);

        ISieveProcessor SieveProcessor { get; }
    }
}