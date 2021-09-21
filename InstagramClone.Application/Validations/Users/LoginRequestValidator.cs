using FluentValidation;
using InstagramClone.Application.Models.Authentificate;

namespace InstagramClone.Application.Validations.Users
{
	public class LoginRequestValidator : AbstractValidator<LoginRequest>
	{
		public LoginRequestValidator()
		{
			RuleFor(x => x.UserName).NotEmpty().EmailAddress();
			RuleFor(x => x.Password).NotEmpty();
		}
	}
}
