using System;
using System.Linq;
using InstagramClone.Domain.Constants.JwtConstants;
using InstagramClone.Domain.UserProviders;
using Microsoft.AspNetCore.Http;

namespace InstagramClone.Application.Services.User.Providers
{
    public class AuthenticatedCurrentUserInfoProvider : IAuthenticatedCurrentUserInfoProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticatedCurrentUserInfoProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public AuthenticatedUserInfo Get()
        {
            var authenticatedUserInfo = new AuthenticatedUserInfo()
            {
                UserName = _httpContextAccessor?.HttpContext?.User?.Identity?.Name,
                Token = _httpContextAccessor?.HttpContext?.User?.Claims?.FirstOrDefault(c => c.Type == JwtClaimsNamesConstants.JwtClaimName)?.Value
            };

            return authenticatedUserInfo;
        }
    }
}