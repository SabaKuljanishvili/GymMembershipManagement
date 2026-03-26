using GymMembershipManagement.SERVICE.DTOs.User;
using GymMembershipManagement.SERVICE.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymMembershipManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            await _userService.UserRegistration(model);
            return Ok("User registered successfully");
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login([FromQuery] string username, [FromQuery] string password)
        {
            var user = await _userService.Login(username, password);
            return Ok(user);
        }

        [HttpGet("GetProfile/{userId:int}")]
        public async Task<ActionResult<UserDTO>> GetProfile(int userId)
        {
            var user = await _userService.GetProfile(userId);
            return Ok(user);
        }

        [HttpPut("UpdateProfile/{userId:int}")]
        public async Task<IActionResult> UpdateProfile(int userId, [FromBody] UpdateUserModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            await _userService.UpdateProfile(userId, model);
            return Ok();
        }

        [HttpDelete("DeleteProfile/{userId:int}")]
        public async Task<IActionResult> DeleteProfile(int userId)
        {
            await _userService.DeleteProfile(userId);
            return Ok();
        }

        [HttpPost("Logout")]
        public IActionResult Logout()
        {
            _userService.Logout();
            return Ok("Logged out");
        }
    }
}
