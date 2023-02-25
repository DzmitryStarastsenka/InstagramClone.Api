using InstagramClone.Domain.DAL.Models.User;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InstagramClone.Domain.DAL.Models.Post
{
    public class PostComment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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