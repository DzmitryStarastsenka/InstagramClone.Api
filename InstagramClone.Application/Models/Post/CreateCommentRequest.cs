namespace InstagramClone.Application.Models.Post
{
    public class CreateCommentRequest
    {
        public string Description { get; set; }
        public int PostId { get; set; }
    }
}