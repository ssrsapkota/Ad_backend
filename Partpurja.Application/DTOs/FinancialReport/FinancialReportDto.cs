namespace Partpurja.Application.DTOs.FinancialReport
{
    
    public enum ReportPeriod
    {
        Daily,
        Monthly,
        Yearly
    }

    
    public class FinancialReportDto
    {
        public ReportPeriod Period { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

        // Revenue (Sales)
        public decimal TotalRevenue { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalCreditAmount { get; set; }
        public decimal TotalPaidRevenue { get; set; }
        public int TotalSalesInvoices { get; set; }

        // Purchases
        public decimal TotalPurchases { get; set; }
        public decimal TotalPurchasesPaid { get; set; }
        public int TotalPurchaseInvoices { get; set; }

        // Net
        public decimal GrossProfit => TotalRevenue - TotalPurchases;

        public List<SalesSummaryDto> SalesBreakdown { get; set; } = new();
        public List<PurchaseSummaryDto> PurchaseBreakdown { get; set; } = new();
    }

    public class SalesSummaryDto
    {
        public string Label { get; set; } = string.Empty;   // e.g. "2026-05-18" / "May 2026" / "2026"
        public int InvoiceCount { get; set; }
        public decimal Revenue { get; set; }
        public decimal Discount { get; set; }
        public decimal CreditAmount { get; set; }
    }

    public class PurchaseSummaryDto
    {
        public string Label { get; set; } = string.Empty;
        public int InvoiceCount { get; set; }
        public decimal TotalCost { get; set; }
    }
}