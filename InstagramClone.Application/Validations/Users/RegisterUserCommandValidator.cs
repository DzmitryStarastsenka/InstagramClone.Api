using InstagramClone.Application.Commands.User;
using FluentValidation;
using InstagramClone.Application.Extensions;
using InstagramClone.Application.Services.User.Extensions;
using InstagramClone.Application.Validations.Users.ErrorMessages;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Validations.Users
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
    {
        private readonly IRepository<UserProfile> _userProfileRepository;

        public RegisterUserCommandValidator(IRepository<UserProfile> userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;

            RuleFor(x => x.Request.UserName).NotEmpty().WithValidationErrorCode(ValidationErrorCode.Invalid)
                .WithMessage(UserValidationErrorMessages.EmptyUserName);
            RuleFor(x => x.Request.FirstName).NotEmpty().WithValidationErrorCode(ValidationErrorCode.Invalid)
                .WithMessage(UserValidationErrorMessages.EmptyFirstName);
            RuleFor(x => x.Request.LastName).NotEmpty().WithValidationErrorCode(ValidationErrorCode.Invalid)
                .WithMessage(UserValidationErrorMessages.EmptyLastName);
            RuleFor(x => x.Request.Password).NotEmpty().WithValidationErrorCode(ValidationErrorCode.Invalid)
                .WithMessage(UserValidationErrorMessages.EmptyPassword);

            RuleFor(x => x.Request.UserName).EmailAddress().WithValidationErrorCode(ValidationErrorCode.Invalid)
                .WithMessage(UserValidationErrorMessages.IncorrectEmail);
            RuleFor(x => x.Request.Password).Matches("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{8,}$").WithValidationErrorCode(ValidationErrorCode.Invalid)
                .WithMessage(UserValidationErrorMessages.WeakPassword);
            RuleFor(x => x.Request.UserName).MustAsync(UserNotExistsAsync).WithValidationErrorCode(ValidationErrorCode.AlreadyExists)
                .WithMessage(UserValidationErrorMessages.UserAlreadyExists);
        }

        private async Task<bool> UserNotExistsAsync(string userName, CancellationToken token)
        {
            return !await _userProfileRepository.UserExistsAsync(userName, token);
        }
    }
}