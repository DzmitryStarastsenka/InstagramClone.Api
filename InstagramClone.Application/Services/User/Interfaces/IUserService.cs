using InstagramClone.Application.Models.Authentificate;
using InstagramClone.Application.Models.User;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstagramClone.Application.Services.User.Interfaces
{
    public interface IUserService
    {
        Task<UserProfileDTO> AuthenticateAsync(LoginRequest loginRequest);
        Task<List<UserProfileDTO>> GetUserProfiles(SieveModel request);
        Task<UserProfileDTO> CreateAsync(UserProfileDTO user, string password);
        UserProfileDTO GetById(int id);
        Task<UserProfileDTO> GetByUserNameAsync(string userName);
        Task<bool> UserExistsAsync(string userName);
        Task UpdateAsync(UserProfileDTO user, string password = null);
        string GenerateToken(UserProfileDTO user);
        bool ValidateToken(string token);
    }
}