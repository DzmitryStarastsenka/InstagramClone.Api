using InstagramClone.Application.Models.Post;
using InstagramClone.Application.Services.Post.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System.Threading.Tasks;

namespace InstagramClone.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet]
        [Route("posts")]
        public async Task<IActionResult> GetPosts([FromQuery] SieveModel request)
        {
            var posts = await _postService.GetPostsAsync(request);
            return Ok(posts);
        }

        [HttpGet]
        [Route("comments")]
        public async Task<IActionResult> GetComments([FromQuery] SieveModel request)
        {
            var comments = await _postService.GetCommentsAsync(request);
            return Ok(comments);
        }

        [HttpGet]
        [Route("likesCount")]
        public async Task<IActionResult> GetLikesCount([FromQuery] SieveModel request)
        {
            var comments = await _postService.GetLikesCountAsync(request);
            return Ok(comments);
        }

        [HttpGet("{id}")]
        public IActionResult GetPostById([FromRoute] int id)
        {
            return Ok(_postService.GetPostById(id));
        }

        [HttpGet("comment/{id}")]
        public IActionResult GetCommentById([FromRoute] int id)
        {
            return Ok(_postService.GetCommentById(id));
        }

        [HttpGet("{id}/liked")]
        public async Task<IActionResult> IsPostLiked([FromRoute] int id)
        {
            return Ok(await _postService.IsPostLikedAsync(id));
        }

        [HttpPost("createPost")]
        public async Task<IActionResult> CreateUserPost([FromBody] CreatePostRequest request)
        {
            await _postService.CreateUserPostAsync(request);
            return Ok();
        }

        [HttpPost("createComment")]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentRequest request)
        {
            await _postService.CreateCommentAsync(request);
            return Ok();
        }


        [HttpPut("{id}/like")]
        public async Task<IActionResult> LikePost([FromRoute] int id)
        {
            await _postService.LikePostAsync(id);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromBody] UpdatePostRequest request)
        {
            await _postService.UpdateUserPostAsync(request);
            return Ok();
        }

        [HttpPut("{id}/updatePhoto")]
        public async Task<IActionResult> UpdatePostPhoto([FromRoute] int id, [FromBody] UpdatePostPhotoRequest request)
        {
            await _postService.UpdatePostPhotoAsync(request);
            return Ok();
        }

        [HttpPut("comment/{id}")]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] EditCommentRequest request)
        {
            await _postService.EditCommentAsync(request);
            return Ok();
        }

        [HttpPut("post/{id}/unlike")]
        public async Task<IActionResult> UnlikePost([FromRoute] int id)
        {
            await _postService.UnlikePostAsync(id);
            return Ok();
        }
    }
}