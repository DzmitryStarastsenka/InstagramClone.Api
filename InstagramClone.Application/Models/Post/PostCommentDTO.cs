using InstagramClone.Application.Models.User;
using System;

namespace InstagramClone.Application.Models.Post
{
    public class PostCommentDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime CommentCreatedAt { get; set; }
        public bool IsEdited { get; set; }
        public DateTime? CommentEditedAt { get; set; }
        public int UserProfileId { get; set; }
        public UserProfileDTO UserProfile { get; set; }
        public int PostId { get; set; }
        public UserPostDTO Post { get; set; }
    }
}