using AutoMapper;
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
    public class CreateCommentCommand : IRequest<CommentCreationResponse>
    {
        public CreateCommentCommand(int postId, CreateCommentRequest request)
        {
            PostId = postId;
            Request = request;
        }

        public int PostId { get; }
        public CreateCommentRequest Request { get; }
    }

    public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, CommentCreationResponse>
    {
        private readonly IRepository<PostComment> _postCommentRepository;
        private readonly IAuthenticatedCurrentUserInfoProvider _authenticatedCurrentUserInfoProvider;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IMapper _mapper;

        public CreateCommentCommandHandler(IRepository<PostComment> postCommentRepository,
            IAuthenticatedCurrentUserInfoProvider authenticatedCurrentUserInfoProvider,
            IRepository<UserProfile> userProfileRepository, IMapper mapper)
        {
            _postCommentRepository = postCommentRepository;
            _authenticatedCurrentUserInfoProvider = authenticatedCurrentUserInfoProvider;
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }

        public async Task<CommentCreationResponse> Handle(CreateCommentCommand command, CancellationToken cancellationToken)
        {
            var request = command.Request;
            var postId = command.PostId;

            var currentUserName = _authenticatedCurrentUserInfoProvider.Get().UserName;
            var currentUserId = await _userProfileRepository.GetUserIdByUserName(currentUserName, cancellationToken);

            var postComment = _mapper.Map<PostComment>(request);
            postComment.PostId = postId;
            postComment.UserProfileId = currentUserId;

            await _postCommentRepository.InsertAsync(postComment, cancellationToken);
            await _postCommentRepository.SaveChangesAsync(cancellationToken);

            return new CommentCreationResponse
            {
                Id = postComment.Id
            };
        }
    }
}