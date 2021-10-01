using AutoMapper;
using InstagramClone.Application.Models.Authentificate;
using InstagramClone.Application.Models.User;
using InstagramClone.Application.Services.User.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace InstagramClone.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthentificationController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public AuthentificationController(
            IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var user = await _userService.AuthenticateAsync(request);

            if (user == null)
                return BadRequest();

            var token = _userService.GenerateToken(user);

            return new LoginResponse
            {
                UserProfile = user,
                Token = token
            };
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            var userDTO = _mapper.Map<UserProfileDTO>(request);
            await _userService.CreateAsync(userDTO, request.Password);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("validateToken")]
        public IActionResult ValidateToken([FromBody] ValidateTokenRequest request)
        {
            var validateToken = _userService.ValidateToken(request.Token);
            if (validateToken)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}