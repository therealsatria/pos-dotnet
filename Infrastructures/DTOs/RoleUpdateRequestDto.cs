using System.ComponentModel.DataAnnotations;

namespace Infrastructures.DTOs
{
    public class RoleUpdateRequestDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Role name must be between 2 and 50 characters")]
        public required string Name { get; set; }
    }
}