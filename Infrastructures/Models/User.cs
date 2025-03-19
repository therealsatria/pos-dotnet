using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Infrastructures.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public required string Username { get; set; }
        
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        
        [Required]
        [ForeignKey("Role")]
        public Guid RoleId { get; set; }
        
        public Role? Role { get; set; }
    }
}