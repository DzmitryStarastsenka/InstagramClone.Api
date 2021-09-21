namespace InstagramClone.Application.Models.User
{
    public class LoginResponse
    {
        public UserProfileDTO UserProfile { get; set; }
        public string Token { get; set; }
    }
}