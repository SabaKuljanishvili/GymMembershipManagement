using GymMembershipManagement.SERVICE.DTOs.Role;
using GymMembershipManagement.SERVICE.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GymMembershipManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _Roleservice;
        public RoleController(IRoleService roleService)
        {
            _Roleservice = roleService;
        }

        [HttpGet("GetAllRoles")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAllRoles()
        {
            var roles = await _Roleservice.GetAllRolesAsync();
            return Ok(roles);
        }

        [HttpGet("GetRoleById/{id}")]
        public async Task<ActionResult<RoleDto>> GetRoleById(int id)
        {
            var role = await _Roleservice.GetRoleByIdAsync(id);
            if (role == null)
                return NotFound();
            return Ok(role);
        }

        [HttpPost("CreateRole")]
        public async Task<ActionResult<RoleDto>> CreateRole(CreateRoleDto model)
        {
            var role = await _Roleservice.CreateRoleAsync(model);
            return CreatedAtAction(nameof(GetRoleById), new { id = role.RoleId }, role);
        }

        [HttpPut("UpdateRoleDto/{id}")]
        public async Task<IActionResult> UpdateRole(int id, UpdateRoleDto model)
        {
            var result = await _Roleservice.UpdateRoleAsync(id, model);
            if (!result)
                return NotFound();
            return NoContent();
        }

        [HttpDelete("UpdateRoleDto/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var result = await _Roleservice.DeleteRoleAsync(id);
            if (!result)
                return NotFound();
            return NoContent();
        }
    }
}
