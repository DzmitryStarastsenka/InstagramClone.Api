using FluentValidation;
using InstagramClone.Application.Queries.User;
using InstagramClone.Application.Extensions;
using InstagramClone.Domain.Exceptions;
using InstagramClone.Application.Validations.Posts.ErrorMessages;

namespace InstagramClone.Application.Validations.Users
{
    public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
    {
        public CreatePostCommandValidator()
        {
            RuleFor(x => x.Request.Description).NotEmpty().WithValidationErrorCode(ValidationErrorCode.Required)
                .WithMessage(PostValidationErrorMessages.EmptyDescription);
        }
    }
}