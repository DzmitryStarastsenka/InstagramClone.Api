using Newtonsoft.Json;

namespace InstagramClone.Application.Models.Post.Requests
{
    public class UpdatePostRequest
    {
        [JsonIgnore]
        public int PostId { get; set; }
        public string Description { get; set; }
    }
}