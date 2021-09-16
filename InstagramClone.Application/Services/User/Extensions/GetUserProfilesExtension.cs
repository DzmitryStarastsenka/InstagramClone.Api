using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.User;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using System.Linq;
using System.Threading.Tasks;

namespace InstagramClone.Application.Services.User.Extensions
{
    public static class GetUserProfilesExtension
    {
        public static async Task<UserProfile> GetByUsernameAsync(
            this IRepository<UserProfile> repository, string userName)
        {
            return await repository.Query
                .FirstOrDefaultAsync(x => x.UserName == userName);
        }

        public static IQueryable<UserProfile> FilterUserProfiles(
           this IRepository<UserProfile> repository, SieveModel requestModel)
        {
            var query = repository.Query.AsNoTracking();
            return repository.SieveProcessor.Apply(requestModel, query);
        }
    }
}