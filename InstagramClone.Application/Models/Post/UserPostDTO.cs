using InstagramClone.Application.Models.User;
using Microsoft.AspNetCore.Http;
using System;

namespace InstagramClone.Application.Models.Post
{
    public class UserPostDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime PostCreatedAt { get; set; }
        public DateTime? PostEditedAt { get; set; }
        public IFormFile Photo { get; set; }
        public int CreatedUserProfileId { get; set; }
        public UserProfileDTO CreatedUserProfile { get; set; }
    }
}