using InstagramClone.Domain.UserProviders;

namespace InstagramClone.UnitTests.Services.User.Providers;

public class AuthenticatedFakeUserInfoProvider : IAuthenticatedCurrentUserInfoProvider
{
    public AuthenticatedFakeUserInfoProvider()
    {
    }

    public AuthenticatedUserInfo Get()
    {
        return new AuthenticatedUserInfo()
        {
            UserName = "testLoggedInUser@gmail.com",
            Token = "testToken"
        };
    }
}
