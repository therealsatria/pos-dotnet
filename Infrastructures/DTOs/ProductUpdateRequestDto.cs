using System.ComponentModel.DataAnnotations;

namespace Infrastructures.DTOs
{
    public class ProductUpdateRequestDto
    {
        [Required]
        public Guid Id { get; set; }

        [StringLength(100, MinimumLength = 3)]
        public string? Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? Price { get; set; }

        [Range(0, int.MaxValue)]
        public int? StockQuantity { get; set; }

        [StringLength(50)]
        public string? Category { get; set; }

        [StringLength(20)]
        public string? Barcode { get; set; }
    }
}
