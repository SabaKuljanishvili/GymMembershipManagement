using GymMembershipManagement.SERVICE.DTOs.User;
using GymMembershipManagement.SERVICE.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymMembershipManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("AddUser")]
        public async Task<ActionResult<UserDTO>> AddUser([FromBody] UserRegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = await _adminService.AddUser(model);
            return Ok(user);
        }

        [HttpGet("GetUserById/{userId:int}")]
        public async Task<ActionResult<UserDTO>> GetUserById(int userId)
        {
            var user = await _adminService.GetUserById(userId);
            return Ok(user);
        }

        [HttpPut("UpdateUser/{userId:int}")]
        public async Task<IActionResult> UpdateUserDetails(int userId, [FromBody] UpdateUserModel model)
        {
            if (!ModelState.IsValid) return BadRequest();
            await _adminService.UpdateUserDetails(userId, model);
            return Ok();
        }

        [HttpDelete("RemoveUser/{userId:int}")]
        public async Task<IActionResult> RemoveUser(int userId)
        {
            await _adminService.RemoveUser(userId);
            return Ok();
        }

        [HttpGet("GetAllMembers")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllMembers()
        {
            var members = await _adminService.GetAllMembers();
            return Ok(members);
        }

        [HttpGet("GetAllTrainers")]
        public async Task<ActionResult<IEnumerable<UserDTO>>> GetAllTrainers()
        {
            var trainers = await _adminService.GetAllTrainers();
            return Ok(trainers);
        }
    }
}
