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
    public class AuthenticateUserCommandValidator : AbstractValidator<AuthenticateUserCommand>
    {
        private readonly IRepository<UserProfile> _userProfileRepository;

        public AuthenticateUserCommandValidator(IRepository<UserProfile> userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;

            RuleFor(x => x.Request.UserName).EmailAddress().WithValidationErrorCode(ValidationErrorCode.Invalid)
                .WithMessage(UserValidationErrorMessages.IncorrectEmail);
            RuleFor(x => x.Request.UserName).MustAsync(IsUserExists).WithValidationErrorCode(ValidationErrorCode.NotFound)
                .WithMessage(UserValidationErrorMessages.UserNotFound);
            RuleFor(x => x.Request.Password).NotEmpty().WithValidationErrorCode(ValidationErrorCode.Invalid)
                .WithMessage(UserValidationErrorMessages.IncorrectPassword);
        }

        private async Task<bool> IsUserExists(string userName, CancellationToken token)
        {
            return await _userProfileRepository.UserExistsAsync(userName, token);
        }
    }
}
