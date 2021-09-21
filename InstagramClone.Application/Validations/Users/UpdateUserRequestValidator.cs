using FluentValidation;
using InstagramClone.Application.Models.User;

namespace InstagramClone.Application.Validations.Users
{
	public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
	{
		public UpdateUserRequestValidator()
		{
			RuleFor(x => x.UserName).NotEmpty().EmailAddress();
			RuleFor(x => x.Password).NotEmpty().Length(10, 20);
			RuleFor(x => x.FirstName).NotEmpty();
			RuleFor(x => x.LastName).NotEmpty();
		}
	}
}