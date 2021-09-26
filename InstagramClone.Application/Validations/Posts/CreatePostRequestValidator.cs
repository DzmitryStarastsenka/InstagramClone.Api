using FluentValidation;
using InstagramClone.Application.Models.Post;

namespace InstagramClone.Application.Validations.Users
{
	public class CreatePostRequestValidator : AbstractValidator<CreatePostRequest>
	{
		public CreatePostRequestValidator()
		{
			RuleFor(x => x.Description).NotEmpty();
		}
	}
}
