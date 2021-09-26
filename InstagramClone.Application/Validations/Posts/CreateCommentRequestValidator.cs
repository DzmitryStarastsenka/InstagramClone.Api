using FluentValidation;
using InstagramClone.Application.Models.Post;

namespace InstagramClone.Application.Validations.Users
{
	public class CreateCommentRequestValidator : AbstractValidator<CreateCommentRequest>
	{
		public CreateCommentRequestValidator()
		{
			RuleFor(x => x.Description).NotEmpty();
		}
	}
}
