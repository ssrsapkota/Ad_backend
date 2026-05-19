using System.ComponentModel.DataAnnotations;

namespace Partpurja.Application.DTOs.Part
{
    public class PartDto
    {
        public int Id { get; set; }
        public string PartNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int ReorderLevel { get; set; }
        public int VendorId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreatePartDto
    {
        [Required, MaxLength(50)]
        public string PartNumber { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue, ErrorMessage = "Price cannot be negative.")]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
        public int Stock { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ReorderLevel must be at least 1.")]
        public int ReorderLevel { get; set; } = 10;

        [Required]
        public int VendorId { get; set; }
    }

    public class UpdatePartDto
    {
        [Required, MaxLength(50)]
        public string PartNumber { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Category { get; set; } = string.Empty;

        [MaxLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(1, int.MaxValue)]
        public int ReorderLevel { get; set; }

        [Required]
        public int VendorId { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
