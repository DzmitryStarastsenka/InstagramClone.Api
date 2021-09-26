using AutoMapper;
using InstagramClone.Application.Models.Post;
using InstagramClone.Application.Services.Post.Extensions;
using InstagramClone.Application.Services.Post.Interfaces;
using InstagramClone.Application.Services.User.Interfaces;
using InstagramClone.Application.Validations.Posts.ErrorMessages;
using InstagramClone.Domain.DAL;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.Exceptions;
using InstagramClone.Domain.UserProviders;
using Microsoft.EntityFrameworkCore;
using Sieve.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Application.Services.Post
{
    public class PostService : IPostService
    {
        private readonly IRepository<UserPost> _postRepository;
        private readonly IRepository<PostComment> _postCommentRepository;
        private readonly IRepository<PostLike> _postLikeRepository;
        private readonly IUserService _userService;
        private readonly IAuthenticatedCurrentUserInfoProvider _authenticatedCurrentUserInfoProvider;
        private readonly IMapper _mapper;

        public PostService(IRepository<UserPost> postRepository,
            IRepository<PostComment> postCommentRepository,
            IRepository<PostLike> postLikeRepository,
            IUserService userService,
            IAuthenticatedCurrentUserInfoProvider authenticatedCurrentUserInfoProvider,
            IMapper mapper)
        {
            _postRepository = postRepository;
            _postCommentRepository = postCommentRepository;
            _postLikeRepository = postLikeRepository;
            _userService = userService;
            _authenticatedCurrentUserInfoProvider = authenticatedCurrentUserInfoProvider;
            _mapper = mapper;
        }

        public async Task<List<UserPostDTO>> GetPostsAsync(SieveModel request)
        {
            return _mapper.Map<List<UserPostDTO>>(await _postRepository.FilterPosts(request).ToListAsync());
        }
        public async Task<List<PostCommentDTO>> GetCommentsAsync(SieveModel request)
        {
            return _mapper.Map<List<PostCommentDTO>>(await _postCommentRepository.FilterComments(request).ToListAsync());
        }

        public async Task<int> GetLikesCountAsync(SieveModel request)
        {
            return await _postLikeRepository.LikesCountAsync(request);
        }

        public UserPostDTO GetPostById(int id)
        {
            return _mapper.Map<UserPostDTO>(_postRepository.Find(id));
        }

        public PostCommentDTO GetCommentById(int id)
        {
            return _mapper.Map<PostCommentDTO>(_postCommentRepository.Find(id));
        }

        public async Task<bool> IsPostLikedAsync(int postId)
        {
            return await _postLikeRepository.Query.AsNoTracking()
                .AnyAsync(l => l.PostId == postId && l.UserProfile.UserName == _authenticatedCurrentUserInfoProvider.Get().UserName);
        }

        public async Task<UserPostDTO> CreateUserPostAsync(CreatePostRequest request)
        {
            var userPostDTO = _mapper.Map<UserPostDTO>(request);

            var user = await _userService.GetByUserNameAsync(_authenticatedCurrentUserInfoProvider.Get().UserName);
            userPostDTO.CreatedUserProfileId = user.Id;
            userPostDTO.PostCreatedAt = DateTime.Now;

            _postRepository.Insert(_mapper.Map<UserPost>(userPostDTO));
            await _postRepository.SaveChangesAsync(CancellationToken.None);

            return userPostDTO;
        }

        public async Task<PostCommentDTO> CreateCommentAsync(CreateCommentRequest request)
        {
            var postCommentDTO = _mapper.Map<PostCommentDTO>(request);

            var user = await _userService.GetByUserNameAsync(_authenticatedCurrentUserInfoProvider.Get().UserName);
            postCommentDTO.UserProfileId = user.Id;
            postCommentDTO.CommentCreatedAt = DateTime.Now;

            _postCommentRepository.Insert(_mapper.Map<PostComment>(postCommentDTO));
            await _postCommentRepository.SaveChangesAsync(CancellationToken.None);

            return postCommentDTO;
        }

        public async Task LikePostAsync(int postId)
        {
            var user = await _userService.GetByUserNameAsync(_authenticatedCurrentUserInfoProvider.Get().UserName);

            _postLikeRepository.Insert(_mapper.Map<PostLike>(
                new PostLikeDTO
                {
                    PostId = postId,
                    UserProfileId = user.Id
                }));
            await _postLikeRepository.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<UserPostDTO> UpdateUserPostAsync(UpdatePostRequest request)
        {
            var post = _postRepository.Find(request.Id);
            if (post == null)
                throw new NotFoundApiException(PostErrorMessages.PostNotFound);

            var requestPost = _mapper.Map<UserPostDTO>(request);

            post.Description = requestPost.Description;
            post.PostEditedAt = DateTime.Now;

            _postRepository.Update(post);
            await _postRepository.SaveChangesAsync(CancellationToken.None);

            return _mapper.Map<UserPostDTO>(post);
        }

        public async Task UpdatePostPhotoAsync(UpdatePostPhotoRequest request)
        {
            var post = _postRepository.Find(request.Id);
            if (post == null)
                throw new NotFoundApiException(PostErrorMessages.PostNotFound);

            var requestPost = _mapper.Map<UserPost>(_mapper.Map<UserPostDTO>(request));

            post.Photo = requestPost.Photo;
            post.PostEditedAt = DateTime.Now;

            _postRepository.Update(post);
            await _postRepository.SaveChangesAsync(CancellationToken.None);
        }

        public async Task<PostCommentDTO> EditCommentAsync(EditCommentRequest request)
        {
            var postComment = _postCommentRepository.Find(request.Id);
            if (postComment == null)
                throw new NotFoundApiException(PostErrorMessages.CommentNotFound);

            var requestComment = _mapper.Map<PostCommentDTO>(request);

            postComment.Description = requestComment.Description;
            postComment.CommentEditedAt = DateTime.Now;

            _postCommentRepository.Update(postComment);
            await _postCommentRepository.SaveChangesAsync(CancellationToken.None);

            return _mapper.Map<PostCommentDTO>(postComment);
        }

        public async Task UnlikePostAsync(int postId)
        {
            var user = await _userService.GetByUserNameAsync(_authenticatedCurrentUserInfoProvider.Get().UserName);

            var postLikes = _postLikeRepository.Query.AsNoTracking()
                .Where(l => l.PostId == postId && l.UserProfile.UserName == _authenticatedCurrentUserInfoProvider.Get().UserName);

            _postLikeRepository.Query.RemoveRange(postLikes);
            await _postLikeRepository.SaveChangesAsync(CancellationToken.None);
        }
    }
}