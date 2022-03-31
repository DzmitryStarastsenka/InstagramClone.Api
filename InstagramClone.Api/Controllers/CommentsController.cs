using InstagramClone.Api.Filters;
using InstagramClone.Application.Models.Post;
using InstagramClone.Application.Models.Post.Requests;
using InstagramClone.Application.Queries.User;
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
    [Route("api/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{postId}")]
        public async Task<ActionResult<List<PostCommentListItemDto>>> GetComments([FromRoute] int postId,
            [FromQuery] SieveModel request, CancellationToken token)
        {
            return await _mediator.Send(new GetCommentsQuery(postId, request), token);
        }

        [HttpPost]
        [Route("{postId}")]
        public async Task<IActionResult> CreateComment([FromRoute] int postId,
            [FromBody] CreateCommentRequest request, CancellationToken token)
        {
            var comment = await _mediator.Send(new CreateCommentCommand(postId, request), token);
            return StatusCode(StatusCodes.Status201Created, comment);
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(UserAccessToUpdateCommentFilter))]
        public async Task<IActionResult> UpdateComment([FromRoute] int id, [FromBody] UpdateCommentRequest request,
            CancellationToken token)
        {
            await _mediator.Send(new UpdateCommentCommand(id, request), token);
            return NoContent();
        }
    }
}