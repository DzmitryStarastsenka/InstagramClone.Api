using InstagramClone.Domain.Constants.JwtConstants;
using InstagramClone.Domain.Jwt.Interfaces;
using Microsoft.Extensions.Configuration;

namespace InstagramClone.Domain.Jwt.Impl
{
    public class AuthJwtTokenGenerator : JwtTokenGenerator, IUserJwtTokenGenerator
    {
        public AuthJwtTokenGenerator(IConfiguration configuration) : base(configuration)
        {
        }

        public string GenerateToken(string userName, string fullName)
        {
            return GenerateTokenInner(userName, fullName, JwtExpiresInNamesConstants.AuthExpiresInDays);
        }
    }
}