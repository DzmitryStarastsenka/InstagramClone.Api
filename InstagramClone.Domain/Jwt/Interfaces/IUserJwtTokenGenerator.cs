namespace InstagramClone.Domain.Jwt.Interfaces
{
    public interface IUserJwtTokenGenerator
    {
        public string GenerateToken(string userName, string fullName);

        public bool ValidateToken(string token);
    }
}