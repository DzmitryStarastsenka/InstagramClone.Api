using InstagramClone.Domain.DAL.Models.User;
using System;

namespace InstagramClone.Domain.DAL.Models.Post
{
    public class PostComment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CommentCreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? CommentEditedAt { get; set; }
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public int PostId { get; set; }
        public UserPost Post { get; set; }
    }
}