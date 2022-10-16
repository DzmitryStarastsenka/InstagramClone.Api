using FluentValidation;
using InstagramClone.Application.Commands.User;
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
    public class SubscribeToUserCommandValidator : AbstractValidator<SubscribeToUserCommand>
    {
        private readonly IRepository<UserProfile> _userProfileRepository;

        public SubscribeToUserCommandValidator(IRepository<UserProfile> userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;

            RuleFor(x => x.PublisherId).MustAsync(UserExistsAsync).WithValidationErrorCode(ValidationErrorCode.NotFound)
                .WithMessage(UserValidationErrorMessages.UserNotFound);
        }

        private async Task<bool> UserExistsAsync(int userId, CancellationToken token)
        {
            return await _userProfileRepository.UserExistsAsync(userId, token);
        }
    }
}
