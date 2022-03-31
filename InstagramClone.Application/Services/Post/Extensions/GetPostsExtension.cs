using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Services.Post.Extensions
{
    public static class GetPostsExtension
    {
        public static IQueryable<UserPost> FilterPosts(
           this IRepository<UserPost> repository, SieveModel requestModel)
        {
            var query = repository.Query
                .Include(p => p.CreatedUserProfile)
                .AsNoTracking();

            return repository.SieveProcessor.Apply(requestModel, query);
        }

        public static IQueryable<PostComment> FilterComments(
            this IRepository<PostComment> repository, SieveModel requestModel, int? postId = null)
        {
            var query = repository.Query
                .Include(c => c.UserProfile)
                .Include(c => c.Post)
                .AsNoTracking();

            // Apply filtering and sorting
            query = repository.SieveProcessor.Apply(requestModel, query, null, true, true, false);

            // Filter comments by post
            if (postId is { })
            {
                query = query.Where(c => c.PostId == postId);
            }

            // Apply pagination
            query = repository.SieveProcessor.Apply(requestModel, query, null, false, false, true);

            return query;
        }

        public static Task<int> LikesCountAsync(
            this IRepository<PostLike> repository, SieveModel requestModel)
        {
            var query = repository.Query
                .Include(c => c.UserProfile)
                .Include(c => c.Post)
                .AsNoTracking();

            return repository.SieveProcessor.Apply(requestModel, query).CountAsync();
        }

        public static async Task<bool> PostExistsAsync(
            this IRepository<UserPost> repository, int postId, CancellationToken token)
        {
            return await repository.Query.AnyAsync(x => x.Id == postId, token);
        }

        public static async Task<UserPost> GetByIdAsync(
            this IRepository<UserPost> repository, int postId, CancellationToken token)
        {
            return await repository.Query.FirstAsync(x => x.Id == postId, token);
        }

        public static async Task<bool> CommentExistsAsync(
            this IRepository<PostComment> repository, int commentId, CancellationToken token)
        {
            return await repository.Query.AnyAsync(x => x.Id == commentId, token);
        }

        public static async Task<PostComment> GetByIdAsync(
            this IRepository<PostComment> repository, int commentId, CancellationToken token)
        {
            return await repository.Query.FirstAsync(x => x.Id == commentId, token);
        }
    }
}