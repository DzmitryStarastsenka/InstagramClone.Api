namespace InstagramClone.Domain.UserProviders
{
    public interface IAuthenticatedUserInfoProvider
    {
        AuthenticatedUserInfo Get();
    }
}