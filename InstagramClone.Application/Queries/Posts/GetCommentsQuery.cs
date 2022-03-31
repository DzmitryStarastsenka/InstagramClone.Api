using AutoMapper;
using InstagramClone.Application.Models.Post;
using InstagramClone.Application.Services.Post.Extensions;
using InstagramClone.Application.Services.User.Extensions;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.DAL.Models.User;
using InstagramClone.Domain.UserProviders;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Queries.User
{
    public class GetCommentsQuery : IRequest<List<PostCommentListItemDto>>
    {
        public GetCommentsQuery(int postId, SieveModel request)
        {
            PostId = postId;
            Request = request;
        }

        public int PostId { get; set; }
        public SieveModel Request { get; }
    }

    public class GetCommentsQueryHandler : IRequestHandler<GetCommentsQuery, List<PostCommentListItemDto>>
    {
        private readonly IRepository<PostComment> _postCommentRepository;
        private readonly IAuthenticatedCurrentUserInfoProvider _authenticatedCurrentUserInfoProvider;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IMapper _mapper;

        public GetCommentsQueryHandler(IRepository<PostComment> postCommentRepository,
            IAuthenticatedCurrentUserInfoProvider authenticatedCurrentUserInfoProvider,
            IRepository<UserProfile> userProfileRepository, IMapper mapper)
        {
            _postCommentRepository = postCommentRepository;
            _authenticatedCurrentUserInfoProvider = authenticatedCurrentUserInfoProvider;
            _mapper = mapper;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<List<PostCommentListItemDto>> Handle(GetCommentsQuery command, CancellationToken cancellationToken)
        {
            var currentUserName = _authenticatedCurrentUserInfoProvider.Get().UserName;
            var currentUserId = await _userProfileRepository.GetUserIdByUserName(currentUserName, cancellationToken);

            var comments = _mapper.Map<List<PostCommentListItemDto>>(await _postCommentRepository
                .FilterComments(command.Request, command.PostId)
                .ToListAsync(cancellationToken));

            foreach (var comment in comments.Where(c => c.UserProfileId == currentUserId))
                comment.AllowEdit = true;

            return comments;
        }
    }
}