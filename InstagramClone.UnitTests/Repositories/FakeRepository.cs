using InstagramClone.Domain.DAL;
using InstagramClone.Infrastructure.DAL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Sieve.Services;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.UnitTests.Repositories;

public class FakeRepository<TEntity> : IRepository<TEntity> where TEntity : class
{
    public virtual InstagramCloneDbContext Context { get; set; }

    public virtual DbSet<TEntity> Set { get; set; }

    public virtual ISieveProcessor SieveProcessor { get; set; }

    public DbSet<TEntity> Query => Set;

    public FakeRepository()
    {

    }

    public Task<int> SaveChangesAsync(CancellationToken token)
    {
        return Context.SaveChangesAsync(token);
    }

    public TEntity Find(params object[] keys)
    {
        return Set.Find(keys);
    }

    public ValueTask<TEntity> FindAsync(CancellationToken token, params object[] keys)
    {
        return Set.FindAsync(keys, token);
    }

    public void Insert(TEntity entity)
    {
        Set.Add(entity);
    }

    public async Task InsertAsync(TEntity entity, CancellationToken token)
    {
        await Set.AddAsync(entity, token);
    }

    public void Delete(TEntity entity)
    {
        Set.Remove(entity);
    }

    public void Update(TEntity entity)
    {
        Set.Update(entity);
    }

    public EntityEntry<TEntity> GetEntityEntry(TEntity entity)
    {
        throw new NotImplementedException();
    }
}
