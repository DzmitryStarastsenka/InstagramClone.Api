using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Domain.DAL
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Insert(TEntity entity);

        TEntity Find(params object[] keys);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        DbSet<TEntity> Query { get; }

        Task<int> SaveChangesAsync(CancellationToken token);

        ISieveProcessor SieveProcessor { get; }
    }
}