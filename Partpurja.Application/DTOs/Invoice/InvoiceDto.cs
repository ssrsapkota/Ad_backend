using System.ComponentModel.DataAnnotations;

namespace Partpurja.Application.DTOs.Invoice
{
    public class CreateInvoiceDto
    {
        [Required]
        public int CustomerId { get; set; }

        [Range(0, double.MaxValue)]
        public decimal PaidAmount { get; set; }

        [Required, MinLength(1, ErrorMessage = "Invoice must contain at least one item.")]
        public List<CreateInvoiceItemDto> Items { get; set; } = new();
    }

    public class InvoiceDto
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; }
        public DateTime Date { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool LoyaltyDiscountApplied { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal CreditAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<InvoiceItemDto> Items { get; set; } = new();
    }
}
