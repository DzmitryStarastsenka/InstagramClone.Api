using AutoMapper;
using InstagramClone.Application.Models.Post;
using InstagramClone.Application.Models.User;
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
    public class GetPostsQuery : IRequest<List<PostListItemDto>>
    {
        public GetPostsQuery(SieveModel request)
        {
            Request = request;
        }

        public SieveModel Request { get; }
    }

    public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, List<PostListItemDto>>
    {
        private readonly IRepository<UserPost> _userPostRepository;
        private readonly IAuthenticatedCurrentUserInfoProvider _authenticatedCurrentUserInfoProvider;
        private readonly IRepository<UserProfile> _userProfileRepository;
        private readonly IMapper _mapper;

        public GetPostsQueryHandler(IRepository<UserPost> userPostRepository,
            IAuthenticatedCurrentUserInfoProvider authenticatedCurrentUserInfoProvider,
            IRepository<UserProfile> userProfileRepository,
            IMapper mapper)
        {
            _userPostRepository = userPostRepository;
            _authenticatedCurrentUserInfoProvider = authenticatedCurrentUserInfoProvider;
            _userProfileRepository = userProfileRepository;
            _mapper = mapper;
        }

        public async Task<List<PostListItemDto>> Handle(GetPostsQuery command, CancellationToken cancellationToken)
        {
            var currentUserName = _authenticatedCurrentUserInfoProvider.Get().UserName;
            var currentUserId = await _userProfileRepository.GetUserIdByUserName(currentUserName, cancellationToken);

            return await _userPostRepository.FilterPosts(command.Request)
                .Include(p => p.Comments)
                .Include(p => p.Likes)
                .Select(p => new PostListItemDto
                {
                    Id = p.Id,
                    Description = p.Description,
                    PostCreatedAt = p.PostCreatedAt,
                    PostEditedAt = p.PostEditedAt,
                    CreatedUserProfileId = p.CreatedUserProfileId,
                    CreatedUserProfile = _mapper.Map<UserProfileDto>(p.CreatedUserProfile),
                    CommentsCount = p.Comments.Count,
                    LikesCount = p.Likes.Count,
                    AllowEdit = p.CreatedUserProfileId == currentUserId,
                    IsLiked = p.Likes.Any(l => l.UserProfileId == currentUserId),
                }).ToListAsync(cancellationToken);
        }
    }
}