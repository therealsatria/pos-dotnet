using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Infrastructures.Models
{
    public class Role
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public required string Name { get; set; }
        
        [JsonIgnore]
        public ICollection<User>? Users { get; set; }
    }
}