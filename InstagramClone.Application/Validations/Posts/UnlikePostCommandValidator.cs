using FluentValidation;
using InstagramClone.Application.Queries.User;
using InstagramClone.Application.Extensions;
using InstagramClone.Domain.Exceptions;
using InstagramClone.Application.Validations.Posts.ErrorMessages;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using System.Threading.Tasks;
using System.Threading;
using InstagramClone.Application.Services.Post.Extensions;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.UserProviders;
using InstagramClone.Application.Services.User.Extensions;
using Microsoft.EntityFrameworkCore;

namespace InstagramClone.Application.Validations.Users
{
    public class UnlikePostCommandValidator : AbstractValidator<UnlikePostCommand>
    {
        private readonly IRepository<PostLike> _postLikeRepository;
        private readonly IRepository<UserPost> _userPostRepository;
        private readonly IAuthenticatedCurrentUserInfoProvider _authenticatedCurrentUserInfoProvider;
        private readonly IRepository<UserProfile> _userProfileRepository;

        public UnlikePostCommandValidator(IRepository<PostLike> postLikeRepository,
            IRepository<UserPost> userPostRepository,
            IAuthenticatedCurrentUserInfoProvider authenticatedCurrentUserInfoProvider,
            IRepository<UserProfile> userProfileRepository)
        {
            _postLikeRepository = postLikeRepository;
            _userPostRepository = userPostRepository;
            _authenticatedCurrentUserInfoProvider = authenticatedCurrentUserInfoProvider;
            _userProfileRepository = userProfileRepository;

            RuleFor(x => x.Request.PostId).MustAsync(IsPostExists).WithValidationErrorCode(ValidationErrorCode.NotFound)
                .WithMessage(PostValidationErrorMessages.PostNotFound);
            RuleFor(x => x.Request.PostId).MustAsync(IsPostLiked).WithValidationErrorCode(ValidationErrorCode.Invalid)
                .WithMessage(PostValidationErrorMessages.NotLiked);
        }

        private async Task<bool> IsPostExists(int postId, CancellationToken token)
        {
            return await _userPostRepository.PostExistsAsync(postId, token);
        }

        private async Task<bool> IsPostLiked(int postId, CancellationToken token)
        {
            var currentUserName = _authenticatedCurrentUserInfoProvider.Get().UserName;
            var currentUserId = await _userProfileRepository.GetUserIdByUserName(currentUserName, token);

            return await _postLikeRepository.Query.AnyAsync(l => l.PostId == postId && l.UserProfileId == currentUserId, token);
        }
    }
}