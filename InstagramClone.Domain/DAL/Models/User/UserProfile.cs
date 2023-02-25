using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InstagramClone.Domain.DAL.Models.User
{
    public class UserProfile
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        public UserRole Role { get; set; } = UserRole.Client;

        public List<UserPost> Posts { get; } = new List<UserPost>();
        public List<PostComment> Comments { get; } = new List<PostComment>();
        public List<PostLike> Likes { get; } = new List<PostLike>();

        public List<Subscription> Subscriptions { get; } = new List<Subscription>();
        public List<Subscription> Signatories { get; } = new List<Subscription>();
    }
}