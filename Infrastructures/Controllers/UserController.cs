// UserController.cs
using Microsoft.AspNetCore.Mvc;
using Infrastructures.DTOs;
using Infrastructures.Models;
using Infrastructures.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructures.Controllers
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
        public async Task<ActionResult<ResultDto<User>>> GetUser(Guid id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new ResultDto<User> { Success = false, Message = "User not found.", Data = null });
                }
                return Ok(new ResultDto<User> { Success = true, Data = user, Message = "User found successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<User> { Success = false, Message = ex.Message, Data = null });
            }
        }

        [HttpGet]
        public async Task<ActionResult<ResultDto<IEnumerable<User>>>> GetUsers()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                return Ok(new ResultDto<IEnumerable<User>> { Success = true, Data = users, Message = "Users retrieved successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<IEnumerable<User>> { Success = false, Message = ex.Message, Data = null });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResultDto<User>>> CreateUser(UserCreateRequestDto request)
        {
            try
            {
                var user = new User
                {
                    Id = Guid.NewGuid(),
                    Username = request.Username,
                    Email = request.Email,
                    RoleId = request.RoleId,
                    Role = null!
                };
                await _userService.AddAsync(user);
                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, new ResultDto<User> { Success = true, Data = user, Message = "User created successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<User> { Success = false, Message = ex.Message, Data = null });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ResultDto<User>>> UpdateUser(Guid id, UserUpdateRequestDto request)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new ResultDto<User> { Success = false, Message = "User not found.", Data = null });
                }
                await _userService.UpdateUserAsync(user, request);
                return Ok(new ResultDto<User> { Success = true, Data = user, Message = "User updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<User> { Success = false, Message = ex.Message, Data = null });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ResultDto<bool>>> DeleteUser(Guid id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new ResultDto<bool> { Success = false, Message = "User not found.", Data = false });
                }
                await _userService.DeleteAsync(id);
                return Ok(new ResultDto<bool> { Success = true, Data = true, Message = "User deleted successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResultDto<bool> { Success = false, Message = ex.Message, Data = false });
            }
        }
    }
}