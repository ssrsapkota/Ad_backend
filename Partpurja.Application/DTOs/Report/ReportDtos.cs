namespace Partpurja.Application.DTOs.Report
{
    public class FinancialReportDto
    {
        public DateTime PeriodStart { get; set; }
        public DateTime PeriodEnd { get; set; }
        public string PeriodType { get; set; } = string.Empty; // Daily / Monthly / Yearly
        public int InvoiceCount { get; set; }
        public decimal GrossSales { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal NetSales { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal TotalCredit { get; set; }
    }

    public class InventoryReportItemDto
    {
        public int PartId { get; set; }
        public string PartNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Stock { get; set; }
        public int ReorderLevel { get; set; }
        public bool IsLowStock { get; set; }
    }

    public class RegularCustomerDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public int PurchaseCount { get; set; }
    }

    public class HighSpenderDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public decimal TotalSpent { get; set; }
    }

    public class PendingCreditDto
    {
        public int CustomerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal OutstandingCredit { get; set; }
        public int InvoiceCount { get; set; }
        public DateTime OldestInvoiceDate { get; set; }
    }
}
