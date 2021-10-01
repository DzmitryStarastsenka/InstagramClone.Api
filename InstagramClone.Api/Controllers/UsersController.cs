using AutoMapper;
using InstagramClone.Application.Models.User;
using InstagramClone.Application.Services.User.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sieve.Models;
using System.Threading.Tasks;

namespace InstagramClone.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UsersController(
            IUserService userService,
            IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("userProfiles")]
        public async Task<IActionResult> GetUserProfiles([FromQuery] SieveModel request)
        {
            var users = await _userService.GetUserProfiles(request);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            return Ok(_userService.GetById(id));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateUserRequest request)
        {
            var user = _mapper.Map<UserProfileDTO>(request);
            user.Id = id;

            await _userService.UpdateAsync(user, request.Password);
            return Ok();
        }
    }
}