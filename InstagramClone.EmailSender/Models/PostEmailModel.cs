using InstagramClone.Application.Models.User;
using System;

namespace InstagramClone.EmailSender.Models
{
    public class PostEmailModel
    {
        public string Description { get; set; }
        public DateTime PostCreatedAt { get; set; }
        public UserProfileDto UserProfile { get; set; }
    }
}
