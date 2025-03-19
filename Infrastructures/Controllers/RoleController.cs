using Microsoft.AspNetCore.Mvc;
using Infrastructures.DTOs;
using Infrastructures.Models;
using Infrastructures.Services;
using Infrastructures.ResponseBuilder;
using Infrastructures.Exceptions;

namespace YourWebApiProject.Infrastructures.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoleController : ControllerBase
    {
        private readonly RoleService _roleService;

        public RoleController(RoleService roleService)
        {
            _roleService = roleService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRole(Guid id)
        {
            try
            {
                var role = await _roleService.GetByIdAsync(id);
                return ResponseBuilder.Success(role);
            }
            catch (NotFoundException ex)
            {
                return ResponseBuilder.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseBuilder.Error(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetRoles()
        {
            try
            {
                var roles = await _roleService.GetAllAsync();
                return ResponseBuilder.Success(roles);
            }
            catch (Exception ex)
            {
                return ResponseBuilder.Error(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleCreateRequestDto request)
        {
            try
            {
                var role = new Role
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name
                };
                await _roleService.AddAsync(role);
                return CreatedAtAction(nameof(GetRole), new { id = role.Id }, ResponseBuilder.Success(role, "Role created successfully."));
            }
            catch (ValidationException ex)
            {
                return ResponseBuilder.Error(ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseBuilder.Error(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRole(Guid id, RoleUpdateRequestDto request)
        {
            try
            {
                var role = await _roleService.GetByIdAsync(id);
                role.Name = request.Name;
                await _roleService.UpdateRoleAsync(role, request);
                return ResponseBuilder.Success(role, "Role updated successfully.");
            }
            catch (NotFoundException ex)
            {
                return ResponseBuilder.NotFound(ex.Message);
            }
            catch (ValidationException ex)
            {
                return ResponseBuilder.Error(ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseBuilder.Error(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(Guid id)
        {
            try
            {
                await _roleService.DeleteAsync(id);
                return ResponseBuilder.Success(true, "Role deleted successfully.");
            }
            catch (NotFoundException ex)
            {
                return ResponseBuilder.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return ResponseBuilder.Error(ex.Message);
            }
        }
    }
}