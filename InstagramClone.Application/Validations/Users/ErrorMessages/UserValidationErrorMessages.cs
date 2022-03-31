namespace InstagramClone.Application.Validations.Users.ErrorMessages
{
    public class UserValidationErrorMessages
    {
        public const string EmptyUserName = "UserName can not be empty.";
        public const string IncorrectEmail = "Incorrect UserName.";
        public const string EmptyFirstName = "First name can not be empty.";
        public const string EmptyLastName = "Last name can not be empty.";
        public const string EmptyPassword = "Password can not be empty.";
        public const string WeakPassword = "The password must contain at least one uppercase letter, one lowercase letter, and one number. Also, the password length cannot be less than 8 characters.";

        public const string UserAlreadyExists = "UserName \"{0}\" is already taken.";
        public const string UserNotFound = "User not found.";
        public const string IncorrectPassword = "Incorrect password.";
    }
}