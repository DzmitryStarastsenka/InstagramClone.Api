using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.DAL.Models.User;
using Microsoft.EntityFrameworkCore;

namespace InstagramClone.Infrastructure.DAL.Context
{
    public class InstagramCloneDbContext : DbContext
    {
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserPost> Posts { get; set; }
        public DbSet<PostLike> Likes { get; set; }
        public DbSet<PostComment> Comments { get; set; }

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

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PostComment>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.UserProfile)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<PostLike>(entity =>
            {
                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(d => d.UserProfile)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.UserProfileId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserPost>(entity =>
            {
                entity.HasOne(d => d.CreatedUserProfile)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.CreatedUserProfileId)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });

            modelBuilder.Entity<PostLike>(entity =>
            {
                entity.HasKey(e => new { e.UserProfileId, e.PostId });
            });
        }
    }
}