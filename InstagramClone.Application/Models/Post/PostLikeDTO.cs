using InstagramClone.Application.Models.User;

namespace InstagramClone.Application.Models.Post
{
    public class PostLikeDTO
    {
        public int UserProfileId { get; set; }
        public UserProfileDTO UserProfile { get; set; }
        public int PostId { get; set; }
        public UserPostDTO Post { get; set; }
    }
}