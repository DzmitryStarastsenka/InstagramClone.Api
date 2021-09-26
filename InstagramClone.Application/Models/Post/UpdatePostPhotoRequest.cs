using Microsoft.AspNetCore.Http;

namespace InstagramClone.Application.Models.Post
{
    public class UpdatePostPhotoRequest
    {
        public int Id { get; set; }
        public IFormFile Photo { get; set; }
    }
}