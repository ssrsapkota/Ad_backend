using System.ComponentModel.DataAnnotations;

namespace Partpurja.Application.DTOs.PurchaseInvoice
{
    public class CreatePurchaseInvoiceDto
    {
        [Required]
        public int VendorId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PaidAmount { get; set; }

        [Range(0, double.MaxValue)]
        public decimal DiscountAmount { get; set; }

        [Required, MinLength(1, ErrorMessage = "Purchase invoice must contain at least one item.")]
        public List<CreatePurchaseInvoiceItemDto> Items { get; set; } = new();
    }

    public class CreatePurchaseInvoiceItemDto
    {
        [Required]
        public int PartId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Unit price cannot be negative.")]
        public decimal UnitPrice { get; set; }
    }

    public class PurchaseInvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<PurchaseInvoiceItemDto> Items { get; set; } = new();
    }

    public class PurchaseInvoiceItemDto
    {
        public int Id { get; set; }
        public int PartId { get; set; }
        public string PartName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
