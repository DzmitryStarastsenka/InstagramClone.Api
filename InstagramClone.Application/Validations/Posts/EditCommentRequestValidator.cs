using FluentValidation;
using InstagramClone.Application.Models.Post;

namespace InstagramClone.Application.Validations.Users
{
	public class EditCommentRequestValidator : AbstractValidator<EditCommentRequest>
	{
		public EditCommentRequestValidator()
		{
			RuleFor(x => x.Description).NotEmpty();
		}
	}
}