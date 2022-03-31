using FluentValidation;
using InstagramClone.Application.Services.User.Extensions;
using InstagramClone.Application.Validations.Users.ErrorMessages;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.User;
using System.Threading;
using System.Threading.Tasks;
using InstagramClone.Application.Queries.User;
using InstagramClone.Application.Extensions;
using InstagramClone.Domain.Exceptions;

namespace InstagramClone.Application.Validations.Users
{
    public class GetUserInfoQueryValidator : AbstractValidator<GetUserInfoQuery>
    {
        private readonly IRepository<UserProfile> _userProfileRepository;

        public GetUserInfoQueryValidator(IRepository<UserProfile> userProfileRepository)
        {
            _userProfileRepository = userProfileRepository;

            RuleFor(x => x.UserId).MustAsync(IsUserExists).WithValidationErrorCode(ValidationErrorCode.NotFound)
                .WithMessage(UserValidationErrorMessages.UserNotFound);
        }

        private async Task<bool> IsUserExists(int userId, CancellationToken token)
        {
            return await _userProfileRepository.UserExistsAsync(userId, token);
        }
    }
}