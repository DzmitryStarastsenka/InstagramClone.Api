using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.DAL.Models.User;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using System.Linq;
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
            this IRepository<PostComment> repository, SieveModel requestModel)
        {
            var query = repository.Query
                .Include(c => c.UserProfile)
                .Include(c => c.Post)
                .AsNoTracking();
            return repository.SieveProcessor.Apply(requestModel, query);
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
    }
}