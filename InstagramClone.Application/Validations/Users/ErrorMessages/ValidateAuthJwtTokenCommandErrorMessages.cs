namespace InstagramClone.Application.Validations.Users.ErrorMessages
{
    public class ValidateAuthJwtTokenCommandErrorMessages
    {
        public const string InvalidToken = "Invalid token.";
        public const string TokenExpired = "The token has expired.";
        public const string EmptyToken = "Token can not be empty.";
        public const string DontHaveAccessToEndpoint = "You don't have permission to access this endpoint.";
    }
}
