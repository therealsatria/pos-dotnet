using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructures.DTOs
{
    public class UserCreateRequestDto
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "RoleId is required")]
        public Guid? RoleId { get; set; }
    }
}