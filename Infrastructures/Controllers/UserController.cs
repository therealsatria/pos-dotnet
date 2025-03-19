using Microsoft.AspNetCore.Mvc;
using Infrastructures.DTOs;
using Infrastructures.Models;
using Infrastructures.Services;
using Infrastructures.ResponseBuilder;
using Infrastructures.Exceptions;
using System;
using System.Threading.Tasks;

namespace YourWebApiProject.Infrastructures.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(Guid id)
        {
            var user = await _userService.GetByIdAsync(id);
            return ResponseBuilder.Success(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAllAsync();
            return ResponseBuilder.Success(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateRequestDto request)
        {
            var user = await _userService.AddAsync(request);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, 
                ResponseBuilder.Success(user, "User created successfully."));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, UserUpdateRequestDto request)
        {
            var user = await _userService.GetByIdAsync(id);
            var updatedUser = await _userService.UpdateUserAsync(user, request);
            return ResponseBuilder.Success(updatedUser, "User updated successfully.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            await _userService.DeleteAsync(id);
            return ResponseBuilder.Success(true, "User deleted successfully.");
        }
    }
}