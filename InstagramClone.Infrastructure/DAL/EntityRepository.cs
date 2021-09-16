using InstagramClone.Domain.DAL;
using InstagramClone.Infrastructure.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Sieve.Services;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Infrastructure.DAL
{
    public sealed class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly InstagramCloneDbContext _context;

        private DbSet<TEntity> Set => _context.Set<TEntity>();

        public DbSet<TEntity> Query => Set;

        public ISieveProcessor SieveProcessor { get; }

        public EntityRepository(InstagramCloneDbContext dbContext, ISieveProcessor sieveProcessor)
        {
            _context = dbContext;
            SieveProcessor = sieveProcessor;
        }

        public TEntity Find(params object[] keys)
        {
            return Set.Find(keys);
        }

        public void Insert(TEntity entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
                Set.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
                Set.Attach(entity);
            Set.Remove(entity);
        }

        public void Update(TEntity entity)
        {
            var entry = _context.Entry(entity);
            if (entry.State == EntityState.Detached)
                Set.Attach(entity);
            entry.State = EntityState.Modified;
        }

        public Task<int> SaveChangesAsync(CancellationToken token)
        {
            return _context.SaveChangesAsync(token);
        }
    }
}