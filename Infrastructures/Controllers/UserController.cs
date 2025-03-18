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
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                return ResponseBuilder.Success(user);
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
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                return ResponseBuilder.Success(users);
            }
            catch (Exception ex)
            {
                return ResponseBuilder.Error(ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateRequestDto request)
        {
            try
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = request.Username,
                    Email = request.Email,
                    RoleId = request.RoleId,
                    Role = null
                };
                await _userService.AddAsync(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, ResponseBuilder.Success(user, "User created successfully."));
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
        public async Task<IActionResult> UpdateUser(Guid id, UserUpdateRequestDto request)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                await _userService.UpdateUserAsync(user, request);
                return ResponseBuilder.Success(user, "User updated successfully.");
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
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _userService.DeleteAsync(id);
                return ResponseBuilder.Success(true, "User deleted successfully.");
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