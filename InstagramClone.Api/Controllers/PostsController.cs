using InstagramClone.Api.Filters;
using InstagramClone.Application.Models.Post;
using InstagramClone.Application.Models.Post.Requests;
using InstagramClone.Application.Queries.User;
using InstagramClone.Domain.Constants.Posts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/posts")]
    public class PostsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<PostListItemDto>>> GetPosts([FromQuery] SieveModel request, CancellationToken token)
        {
            return await _mediator.Send(new GetPostsQuery(request), token);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserPost([FromBody] CreatePostRequest request, CancellationToken token)
        {
            var post = await _mediator.Send(new CreatePostCommand(request), token);
            return StatusCode(StatusCodes.Status201Created, post);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(UserAccessToUpdatePostFilter))]
        public async Task<IActionResult> UpdatePost([FromRoute] int id, [FromBody] UpdatePostRequest request, CancellationToken token)
        {
            await _mediator.Send(new UpdatePostCommand(id, request), token);
            return NoContent();
        }

        [HttpGet]
        [Route("{id}/photo")]
        public async Task<FileContentResult> GetPostPhoto([FromRoute] int id, CancellationToken token)
        {
            var fileBytes = await _mediator.Send(new GetPostPhotoQuery(id), token);
            return File(fileBytes, PostsConstants.CorrectFileType, $"post{id}Photo.png");
        }

        [HttpPut("{id}/photo")]
        [ServiceFilter(typeof(UserAccessToUpdatePostFilter))]
        public async Task<IActionResult> UpdatePostPhoto([FromRoute] int id, [FromForm] IFormFile photo, CancellationToken token)
        {
            await _mediator.Send(new UpdatePostPhotoCommand(id, photo), token);
            return NoContent();
        }

        [HttpPut("{id}/like")]
        public async Task<IActionResult> LikePost([FromRoute] int id, CancellationToken token)
        {
            await _mediator.Send(new LikePostCommand(id), token);
            return Ok();
        }

        [HttpPut("{id}/unlike")]
        public async Task<IActionResult> UnlikePost([FromRoute] int id, CancellationToken token)
        {
            await _mediator.Send(new UnlikePostCommand(id), token);
            return Ok();
        }
    }
}