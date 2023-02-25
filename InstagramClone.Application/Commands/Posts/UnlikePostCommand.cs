using InstagramClone.Application.Models.Post.Requests;
using InstagramClone.Application.Services.User.Extensions;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.UserProviders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Queries.User
{
    public class UnlikePostCommand : IRequest<Unit>
    {
        public UnlikePostCommand(UnlikePostRequest request)
        {
            Request = request;
        }

        public UnlikePostRequest Request { get; }
    }

    public class UnlikePostCommandHandler : IRequestHandler<UnlikePostCommand, Unit>
    {
        private readonly IRepository<PostLike> _postLikeRepository;
        private readonly IAuthenticatedCurrentUserInfoProvider _authenticatedCurrentUserInfoProvider;
        private readonly IRepository<UserProfile> _userProfileRepository;

        public UnlikePostCommandHandler(IRepository<PostLike> postLikeRepository,
            IAuthenticatedCurrentUserInfoProvider authenticatedCurrentUserInfoProvider,
            IRepository<UserProfile> userProfileRepository)
        {
            _postLikeRepository = postLikeRepository;
            _authenticatedCurrentUserInfoProvider = authenticatedCurrentUserInfoProvider;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<Unit> Handle(UnlikePostCommand command, CancellationToken cancellationToken)
        {
            var postId = command.Request.PostId;

            var currentUserName = _authenticatedCurrentUserInfoProvider.Get().UserName;
            var currentUserId = await _userProfileRepository.GetUserIdByUserName(currentUserName, cancellationToken);

            var postLike = await _postLikeRepository.Query.AsNoTracking()
                .Where(l => l.PostId == postId && l.UserProfileId == currentUserId)
                .SingleAsync();

            _postLikeRepository.Query.Remove(postLike);
            await _postLikeRepository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}