using InstagramClone.Domain.DAL.Models.User;

namespace InstagramClone.Domain.DAL.Models.Post
{
    public class PostLike
    {
        public int UserProfileId { get; set; }
        public UserProfile UserProfile { get; set; }
        public int PostId { get; set; }
        public UserPost Post { get; set; }
    }
}
