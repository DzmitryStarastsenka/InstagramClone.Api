using InstagramClone.Application.Models.User;
using System;

namespace InstagramClone.Application.Models.Post
{
    public class PostCommentListItemDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CommentCreatedAt { get; set; }
        public DateTime? CommentEditedAt { get; set; }
        public int UserProfileId { get; set; }
        public UserProfileDto UserProfile { get; set; }
        public bool AllowEdit { get; set; }
    }
}