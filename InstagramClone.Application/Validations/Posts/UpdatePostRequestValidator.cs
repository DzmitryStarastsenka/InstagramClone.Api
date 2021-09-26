using FluentValidation;
using InstagramClone.Application.Models.Post;

namespace InstagramClone.Application.Validations.Users
{
	public class UpdatePostRequestValidator : AbstractValidator<UpdatePostRequest>
	{
		public UpdatePostRequestValidator()
		{
			RuleFor(x => x.Description).NotEmpty();
		}
	}
}