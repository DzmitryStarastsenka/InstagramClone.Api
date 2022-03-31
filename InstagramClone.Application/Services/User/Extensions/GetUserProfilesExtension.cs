using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.User;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Services.User.Extensions
{
    public static class GetUserProfilesExtension
    {
        public static async Task<UserProfile> GetByUserNameAsync(
            this IRepository<UserProfile> repository, string userName, CancellationToken token)
        {
            return await repository.Query
                .FirstOrDefaultAsync(x => x.UserName == userName, token);
        }

        public static IQueryable<UserProfile> FilterUserProfiles(
           this IRepository<UserProfile> repository, SieveModel requestModel)
        {
            var query = repository.Query.AsNoTracking();
            return repository.SieveProcessor.Apply(requestModel, query);
        }

        public static async Task<bool> UserExistsAsync(
            this IRepository<UserProfile> repository, string userName, CancellationToken token)
        {
            return await repository.Query.AnyAsync(x => x.UserName == userName, token);
        }

        public static async Task<bool> UserExistsAsync(
            this IRepository<UserProfile> repository, int userId, CancellationToken token)
        {
            return await repository.Query.AnyAsync(x => x.Id == userId, token);
        }

        public static async Task<int> GetUserIdByUserName(
            this IRepository<UserProfile> repository, string userName, CancellationToken token)
        {
            return await repository.Query
                .Where(u => u.UserName == userName)
                .Select(u => u.Id)
                .SingleAsync(token);
        }
    }
}