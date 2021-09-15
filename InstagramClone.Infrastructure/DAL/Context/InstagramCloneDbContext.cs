using Microsoft.EntityFrameworkCore;

namespace InstagramClone.Infrastructure.DAL.Context
{
    public class InstagramCloneDbContext : DbContext
    {

        public InstagramCloneDbContext(DbContextOptions<InstagramCloneDbContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.LogTo(System.Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}