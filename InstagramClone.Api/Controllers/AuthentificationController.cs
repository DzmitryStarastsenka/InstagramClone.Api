using MediatR;
using InstagramClone.Application.Models.Authentificate;
using InstagramClone.Application.Models.User;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using InstagramClone.Application.Commands.User;
using InstagramClone.Api.Filters;
using Microsoft.AspNetCore.Authorization;

namespace InstagramClone.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthentificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthentificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AnonymousOnly]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken token)
        {
            return await _mediator.Send(new AuthenticateUserCommand(request), token);
        }

        [AnonymousOnly]
        [HttpPost("register")]
        public async Task<ActionResult<UserProfileDto>> Register([FromBody] RegisterRequest request, CancellationToken token)
        {
            return await _mediator.Send(new RegisterUserCommand(request), token);
        }

        [AllowAnonymous]
        [HttpPost("validateToken")]
        public async Task<IActionResult> ValidateTokenAsync([FromBody] string token, CancellationToken cancellationToken)
        {
            await _mediator.Send(new ValidateAuthJwtTokenCommand(token), cancellationToken);
            return Ok();
        }
    }
}