using InstagramClone.Application.Helpers;
using InstagramClone.Application.Services.User.Extensions;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.Models;
using InstagramClone.UnitTests.Helpers;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.UnitTests.Services;

public class CreateHelperService
{
    public static async Task<UserProfile> AddNewTestClientUserAsync(IRepository<UserProfile> userProfileRepository,
        string userName, string password)
    {
        var existingUser = await userProfileRepository.GetByUserNameAsync(userName, CancellationToken.None);

        if (existingUser is { })
            return existingUser;

        PasswordHashHelper.CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

        var testUser = new UserProfile
        {
            FirstName = RandomHelper.GetRandomString(10),
            LastName = RandomHelper.GetRandomString(10),
            UserName = userName,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            Role = UserRole.Client,
        };

        await userProfileRepository.InsertAsync(testUser, CancellationToken.None);
        await userProfileRepository.SaveChangesAsync(CancellationToken.None);

        return testUser;
    }
}
