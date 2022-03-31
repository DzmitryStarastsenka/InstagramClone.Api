using InstagramClone.Application.Models.User;
using System;

namespace InstagramClone.Application.Models.Post
{
    public class PostListItemDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime PostCreatedAt { get; set; }
        public DateTime? PostEditedAt { get; set; }
        public int CreatedUserProfileId { get; set; }
        public UserProfileDto CreatedUserProfile { get; set; }
        
        public int CommentsCount { get; set; }
        public int LikesCount { get; set; }
        public bool AllowEdit { get; set; }
        public bool IsLiked { get; set; }
    }
}
