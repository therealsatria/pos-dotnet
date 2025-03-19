using Microsoft.AspNetCore.Mvc;
using Infrastructures.DTOs;
using Infrastructures.Models;
using Infrastructures.Services;
using Infrastructures.ResponseBuilder;
using System;
using System.Threading.Tasks;

namespace YourWebApiProject.Infrastructures.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(Guid id)
        {
            var role = await _roleService.GetByIdAsync(id);
            return ResponseBuilder.Success(role);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await _roleService.GetAllAsync();
            return ResponseBuilder.Success(roles);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleCreateRequestDto request)
        {
            var role = await _roleService.AddAsync(request);
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, 
                ResponseBuilder.Success(role, "Role created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(Guid id, RoleUpdateRequestDto request)
        {
            var role = await _roleService.GetByIdAsync(id);
            var updatedRole = await _roleService.UpdateRoleAsync(role, request);
            return ResponseBuilder.Success(updatedRole, "Role updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            await _roleService.DeleteAsync(id);
            return ResponseBuilder.Success(true, "Role deleted successfully.");
        }
    }
}