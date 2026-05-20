using Microsoft.EntityFrameworkCore;
using Partpurja.Application.DTOs.FinancialReport;
using Partpurja.Application.Interface.IServices;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Services
{
   
    public class FinancialReportService : IFinancialReportService
    {
        private readonly AppDbContext _context;

        public FinancialReportService(AppDbContext context)
        {
            _context = context;
        }

        //  Daily Report 
        public async Task<FinancialReportDto> GetDailyReportAsync(DateTime date)
        {
            var from = date.Date.ToUniversalTime();
            var to   = from.AddDays(1).AddTicks(-1);

            var report = await BuildReportAsync(ReportPeriod.Daily, from, to);

            // Hourly breakdown is not required, so a single-row summary is returned.
            report.SalesBreakdown = new List<SalesSummaryDto>
            {
                new()
                {
                    Label        = date.ToString("yyyy-MM-dd"),
                    InvoiceCount = report.TotalSalesInvoices,
                    Revenue      = report.TotalRevenue,
                    Discount     = report.TotalDiscount,
                    CreditAmount = report.TotalCreditAmount
                }
            };

            report.PurchaseBreakdown = new List<PurchaseSummaryDto>
            {
                new()
                {
                    Label        = date.ToString("yyyy-MM-dd"),
                    InvoiceCount = report.TotalPurchaseInvoices,
                    TotalCost    = report.TotalPurchases
                }
            };

            return report;
        }

        // Monthly Report 
        public async Task<FinancialReportDto> GetMonthlyReportAsync(int year, int month)
        {
            var from = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
            var to   = from.AddMonths(1).AddTicks(-1);

            var report = await BuildReportAsync(ReportPeriod.Monthly, from, to);

            // Breakdown by day within the month
            var salesByDay = await _context.SalesInvoices
                .Where(si => si.Date >= from && si.Date <= to)
                .GroupBy(si => si.Date.Day)
                .Select(g => new SalesSummaryDto
                {
                    Label        = $"{year}-{month:D2}-{g.Key:D2}",
                    InvoiceCount = g.Count(),
                    Revenue      = g.Sum(si => si.TotalAmount),
                    Discount     = g.Sum(si => si.DiscountAmount),
                    CreditAmount = g.Sum(si => si.CreditAmount)
                })
                .OrderBy(s => s.Label)
                .ToListAsync();

            var purchasesByDay = await _context.PurchaseInvoices
                .Where(pi => pi.Date >= from && pi.Date <= to)
                .GroupBy(pi => pi.Date.Day)
                .Select(g => new PurchaseSummaryDto
                {
                    Label        = $"{year}-{month:D2}-{g.Key:D2}",
                    InvoiceCount = g.Count(),
                    TotalCost    = g.Sum(pi => pi.TotalAmount)
                })
                .OrderBy(s => s.Label)
                .ToListAsync();

            report.SalesBreakdown    = salesByDay;
            report.PurchaseBreakdown = purchasesByDay;

            return report;
        }

        // Yearly Report
        public async Task<FinancialReportDto> GetYearlyReportAsync(int year)
        {
            var from = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var to   = from.AddYears(1).AddTicks(-1);

            var report = await BuildReportAsync(ReportPeriod.Yearly, from, to);

            // Breakdown by month within the year
            var salesByMonth = await _context.SalesInvoices
                .Where(si => si.Date >= from && si.Date <= to)
                .GroupBy(si => si.Date.Month)
                .Select(g => new SalesSummaryDto
                {
                    Label        = $"{year}-{g.Key:D2}",
                    InvoiceCount = g.Count(),
                    Revenue      = g.Sum(si => si.TotalAmount),
                    Discount     = g.Sum(si => si.DiscountAmount),
                    CreditAmount = g.Sum(si => si.CreditAmount)
                })
                .OrderBy(s => s.Label)
                .ToListAsync();

            var purchasesByMonth = await _context.PurchaseInvoices
                .Where(pi => pi.Date >= from && pi.Date <= to)
                .GroupBy(pi => pi.Date.Month)
                .Select(g => new PurchaseSummaryDto
                {
                    Label        = $"{year}-{g.Key:D2}",
                    InvoiceCount = g.Count(),
                    TotalCost    = g.Sum(pi => pi.TotalAmount)
                })
                .OrderBy(s => s.Label)
                .ToListAsync();

            report.SalesBreakdown    = salesByMonth;
            report.PurchaseBreakdown = purchasesByMonth;

            return report;
        }

        //  Shared Aggregate Builder 
        private async Task<FinancialReportDto> BuildReportAsync(
            ReportPeriod period, DateTime from, DateTime to)
        {
            var salesTotals = await _context.SalesInvoices
                .Where(si => si.Date >= from && si.Date <= to)
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    Count        = g.Count(),
                    Revenue      = g.Sum(si => si.TotalAmount),
                    Discount     = g.Sum(si => si.DiscountAmount),
                    Credit       = g.Sum(si => si.CreditAmount),
                    Paid         = g.Sum(si => si.PaidAmount)
                })
                .FirstOrDefaultAsync();

            var purchaseTotals = await _context.PurchaseInvoices
                .Where(pi => pi.Date >= from && pi.Date <= to)
                .GroupBy(_ => 1)
                .Select(g => new
                {
                    Count     = g.Count(),
                    Total     = g.Sum(pi => pi.TotalAmount),
                    Paid      = g.Sum(pi => pi.PaidAmount)
                })
                .FirstOrDefaultAsync();

            return new FinancialReportDto
            {
                Period              = period,
                FromDate            = from,
                ToDate              = to,
                TotalSalesInvoices  = salesTotals?.Count ?? 0,
                TotalRevenue        = salesTotals?.Revenue ?? 0,
                TotalDiscount       = salesTotals?.Discount ?? 0,
                TotalCreditAmount   = salesTotals?.Credit ?? 0,
                TotalPaidRevenue    = salesTotals?.Paid ?? 0,
                TotalPurchaseInvoices = purchaseTotals?.Count ?? 0,
                TotalPurchases      = purchaseTotals?.Total ?? 0,
                TotalPurchasesPaid  = purchaseTotals?.Paid ?? 0
            };
        }
    }
}
