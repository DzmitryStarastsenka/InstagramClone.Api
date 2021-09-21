namespace InstagramClone.Domain.UserProviders
{
    public class AuthenticatedUserInfo
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string TokenType { get; set; }
        public bool IsAuthorized => !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Token);
    }
}