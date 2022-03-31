namespace InstagramClone.Application.Models.User
{
    public class LoginResponse
    {
        public UserProfileDto UserProfile { get; set; }
        public string Token { get; set; }
    }
}