using InstagramClone.Application.Commands.User;
using InstagramClone.Application.Models.User;
using InstagramClone.Application.Queries.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InstagramClone.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("all")]
        public async Task<ActionResult<List<UserProfileDto>>> GetUserProfiles([FromQuery] SieveModel request, CancellationToken token)
        {
            return await _mediator.Send(new GetUserProfilesQuery(request), token);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserProfileDto>> GetById([FromRoute] int id, CancellationToken token)
        {
            return await _mediator.Send(new GetUserInfoQuery(id), token);
        }

        [HttpPost("subscribe/{id}")]
        public async Task<NoContentResult> SubscribeToUser([FromRoute] int id, CancellationToken token)
        {
            await _mediator.Send(new SubscribeToUserCommand(id), token);
            return NoContent();
        }

        //[Authorize(Policy = "Admin")]
        //[HttpPut("{id}")]
        //public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserRequest request)
        //{
        //    var user = _mapper.Map<UserProfileDto>(request);
        //    user.Id = id;

        //    await _userService.UpdateAsync(user, request.Password);
        //    return Ok();
        //}
    }
}