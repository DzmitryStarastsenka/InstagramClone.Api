using InstagramClone.Api.Policies.Requirements;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.Models;
using InstagramClone.Domain.UserProviders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace InstagramClone.Api.Policies.Handlers
{
    public class AdminAuthorizationHandler : AuthorizationHandler<AdminRequirement>
    {
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IAuthenticatedCurrentUserInfoProvider _authenticatedCurrentUserInfoProvider;

        public AdminAuthorizationHandler(IRepository<UserProfile> userProfileRepository,
            IAuthenticatedCurrentUserInfoProvider authenticatedCurrentUserInfoProvider)
        {
            _userProfileRepository = userProfileRepository;
            _authenticatedCurrentUserInfoProvider = authenticatedCurrentUserInfoProvider;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            AdminRequirement requirement)
        {
            var userName = _authenticatedCurrentUserInfoProvider.Get().UserName;

            var userRole = await _userProfileRepository.Query.AsNoTracking()
                .Where(u => u.UserName == userName)
                .Select(u => u.Role)
                .SingleAsync();

            if (userRole == UserRole.Admin) context.Succeed(requirement);
        }
    }
}
