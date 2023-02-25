using InstagramClone.Application.Models.Post.Requests;
using InstagramClone.Application.Services.User.Extensions;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.UserProviders;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Queries.User
{
    public class LikePostCommand : IRequest<Unit>
    {
        public LikePostCommand(LikePostRequest request)
        {
            Request = request;
        }

        public LikePostRequest Request { get; }
    }

    public class LikePostCommandHandler : IRequestHandler<LikePostCommand, Unit>
    {
        private readonly IRepository<PostLike> _postLikeRepository;
        private readonly IAuthenticatedCurrentUserInfoProvider _authenticatedCurrentUserInfoProvider;
        private readonly IRepository<UserProfile> _userProfileRepository;

        public LikePostCommandHandler(IRepository<PostLike> postLikeRepository,
            IAuthenticatedCurrentUserInfoProvider authenticatedCurrentUserInfoProvider,
            IRepository<UserProfile> userProfileRepository)
        {
            _postLikeRepository = postLikeRepository;
            _authenticatedCurrentUserInfoProvider = authenticatedCurrentUserInfoProvider;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<Unit> Handle(LikePostCommand command, CancellationToken cancellationToken)
        {
            var postId = command.Request.PostId;

            var currentUserName = _authenticatedCurrentUserInfoProvider.Get().UserName;
            var currentUserId = await _userProfileRepository.GetUserIdByUserName(currentUserName, cancellationToken);

            await _postLikeRepository.InsertAsync(
                new PostLike
                {
                    PostId = postId,
                    UserProfileId = currentUserId,
                }, cancellationToken);
            await _postLikeRepository.SaveChangesAsync(CancellationToken.None);

            return Unit.Value;
        }
    }
}