using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.DAL.Models.User;
using Microsoft.EntityFrameworkCore;

namespace InstagramClone.Infrastructure.DAL.Context
{
    public class InstagramCloneDbContext : DbContext
    {
        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<UserPost> Posts { get; set; }
        public virtual DbSet<PostLike> Likes { get; set; }
        public virtual DbSet<PostComment> Comments { get; set; }

        public virtual DbSet<Subscription> Subscriptions { get; set; }

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

            modelBuilder.Entity<Subscription>(entity =>
            {
                entity.HasKey(e => new { e.SubscriberId, e.PublisherId });

                entity.HasOne(d => d.Subscriber)
                    .WithMany(p => p.Subscriptions)
                    .HasForeignKey(d => d.SubscriberId)
                    .OnDelete(DeleteBehavior.ClientCascade);

                entity.HasOne(d => d.Publisher)
                    .WithMany(p => p.Signatories)
                    .HasForeignKey(d => d.PublisherId)
                    .OnDelete(DeleteBehavior.ClientCascade);
            });
        }
    }
}