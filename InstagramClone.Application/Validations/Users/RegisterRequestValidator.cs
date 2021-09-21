using FluentValidation;
using InstagramClone.Application.Models.Authentificate;
using InstagramClone.Application.Services.User.Interfaces;
using InstagramClone.Application.Validations.Users.ErrorMessages;
using System.Threading.Tasks;

namespace InstagramClone.Application.Validations.Users
{
	public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
	{
		private readonly IUserService _userService;

		public RegisterRequestValidator(IUserService userService)
		{
			_userService = userService;

			RuleFor(x => x.UserName).NotEmpty().EmailAddress()
				.MustAsync(async (userName, cancellation) => await UserNotExistsAsync(userName))
				.WithMessage(UserErrorMessages.UserAlreadyExists);
			RuleFor(x => x.UserName).NotEmpty().EmailAddress();
			RuleFor(x => x.Password).NotEmpty().Length(10, 20);
			RuleFor(x => x.FirstName).NotEmpty();
			RuleFor(x => x.LastName).NotEmpty();
		}

		private async Task<bool> UserNotExistsAsync(string userName)
		{
			return !await _userService.UserExistsAsync(userName);
		}
	}
}