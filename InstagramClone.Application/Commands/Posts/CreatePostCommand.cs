using AutoMapper;
using InstagramClone.Application.Commands.Notifications;
using InstagramClone.Application.Models.Post.Requests;
using InstagramClone.Application.Models.Post.Responses;
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
    public class CreatePostCommand : IRequest<PostCreationResponse>
    {
        public CreatePostCommand(CreatePostRequest request)
        {
            Request = request;
        }

        public CreatePostRequest Request { get; }
    }

    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, PostCreationResponse>
    {
        private readonly IRepository<UserPost> _userPostRepository;
        private readonly IAuthenticatedCurrentUserInfoProvider _authenticatedCurrentUserInfoProvider;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;

        public CreatePostCommandHandler(IRepository<UserPost> userPostRepository,
            IAuthenticatedCurrentUserInfoProvider authenticatedCurrentUserInfoProvider,
            IRepository<UserProfile> userProfileRepository, IMapper mapper, IMediator mediator)
        {
            _userPostRepository = userPostRepository;
            _authenticatedCurrentUserInfoProvider = authenticatedCurrentUserInfoProvider;
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task<PostCreationResponse> Handle(CreatePostCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;

            var currentUserName = _authenticatedCurrentUserInfoProvider.Get().UserName;
            var currentUserId = await _userProfileRepository.GetUserIdByUserName(currentUserName, cancellationToken);

            var userPost = _mapper.Map<UserPost>(request);
            userPost.CreatedUserProfileId = currentUserId;

            await _userPostRepository.InsertAsync(userPost, cancellationToken);
            await _userPostRepository.SaveChangesAsync(cancellationToken);

            await _mediator.Send(new SendUserSubscriptionNotificationsCommand(new()
            {
                CreatedUserProfileId = currentUserId,
                PostId = userPost.Id
            }), cancellationToken);

            return new PostCreationResponse
            {
                Id = userPost.Id
            };
        }
    }
}