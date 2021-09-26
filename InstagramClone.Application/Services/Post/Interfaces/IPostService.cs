using InstagramClone.Application.Models.Post;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InstagramClone.Application.Services.Post.Interfaces
{
    public interface IPostService
    {
        #region Get
        public Task<List<UserPostDTO>> GetPostsAsync(SieveModel request);
        public Task<List<PostCommentDTO>> GetCommentsAsync(SieveModel request);
        public Task<int> GetLikesCountAsync(SieveModel request);
        public UserPostDTO GetPostById(int id);
        public PostCommentDTO GetCommentById(int id);
        public Task<bool> IsPostLikedAsync(int postId);

        #endregion

        #region Create
        public Task<UserPostDTO> CreateUserPostAsync(CreatePostRequest request);
        public Task<PostCommentDTO> CreateCommentAsync(CreateCommentRequest request);
        public Task LikePostAsync(int postId);
        #endregion

        #region Update
        public Task<UserPostDTO> UpdateUserPostAsync(UpdatePostRequest request);
        public Task UpdatePostPhotoAsync(UpdatePostPhotoRequest request);
        public Task<PostCommentDTO> EditCommentAsync(EditCommentRequest request);
        public Task UnlikePostAsync(int postId);
        #endregion
    }
}
