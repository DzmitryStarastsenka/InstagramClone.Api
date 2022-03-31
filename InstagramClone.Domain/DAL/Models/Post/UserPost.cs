using InstagramClone.Domain.DAL.Models.User;
using System;
using System.Collections.Generic;

namespace InstagramClone.Domain.DAL.Models.Post
{
    public class UserPost
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime PostCreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PostEditedAt { get; set; }

        public byte[] Photo { get; set; }

        public int CreatedUserProfileId { get; set; }
        public UserProfile CreatedUserProfile { get; set; }

        public List<PostLike> Likes { get; } = new List<PostLike>();
        public List<PostComment> Comments { get; } = new List<PostComment>();
    }
}