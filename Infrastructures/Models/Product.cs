using System;
using System.ComponentModel.DataAnnotations;

namespace Infrastructures.Models
{
    public class Product
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3)]
        public required string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int StockQuantity { get; set; }

        [Required]
        [StringLength(50)]
        public required string Category { get; set; }

        [Required]
        [StringLength(20)]
        public required string Barcode { get; set; }
    }
}
