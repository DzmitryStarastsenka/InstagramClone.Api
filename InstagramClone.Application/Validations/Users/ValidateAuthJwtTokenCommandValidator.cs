using InstagramClone.Application.Commands.User;
using FluentValidation;
using InstagramClone.Application.Extensions;
using InstagramClone.Application.Validations.Users.ErrorMessages;
using InstagramClone.Domain.Exceptions;

namespace InstagramClone.Application.Validations.Users
{
    public class ValidateAuthJwtTokenCommandValidator : AbstractValidator<ValidateAuthJwtTokenCommand>
    {
        public ValidateAuthJwtTokenCommandValidator()
        {
            RuleFor(x => x.Token).NotEmpty().WithValidationErrorCode(ValidationErrorCode.Invalid)
                .WithMessage(ValidateAuthJwtTokenCommandErrorMessages.EmptyToken);
        }
    }
}