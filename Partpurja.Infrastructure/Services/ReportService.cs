using Partpurja.Application.DTOs.Report;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Domain.Models;

namespace Partpurja.Infrastructure.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _repo;

        public ReportService(IReportRepository repo)
        {
            _repo = repo;
        }

        public async Task<FinancialReportDto> GetDailyReportAsync(DateTime day)
        {
            var start = DateTime.SpecifyKind(day.Date, DateTimeKind.Utc);
            var end = start.AddDays(1);
            var invoices = await _repo.GetSalesInvoicesInRangeAsync(start, end);
            return BuildFinancialReport(start, end, "Daily", invoices);
        }

        public async Task<FinancialReportDto> GetMonthlyReportAsync(int year, int month)
        {
            if (month < 1 || month > 12)
            {
                throw new InvalidOperationException("Month must be between 1 and 12.");
            }

            var start = new DateTime(year, month, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = start.AddMonths(1);
            var invoices = await _repo.GetSalesInvoicesInRangeAsync(start, end);
            return BuildFinancialReport(start, end, "Monthly", invoices);
        }

        public async Task<FinancialReportDto> GetYearlyReportAsync(int year)
        {
            var start = new DateTime(year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var end = start.AddYears(1);
            var invoices = await _repo.GetSalesInvoicesInRangeAsync(start, end);
            return BuildFinancialReport(start, end, "Yearly", invoices);
        }

        public async Task<List<InventoryReportItemDto>> GetInventoryReportAsync()
        {
            var parts = await _repo.GetAllPartsAsync();
            return parts.Select(p => new InventoryReportItemDto
            {
                PartId = p.Id,
                PartNumber = p.PartNumber,
                Name = p.Name,
                Stock = p.Stock,
                ReorderLevel = p.ReorderLevel,
                IsLowStock = p.Stock < p.ReorderLevel
            }).ToList();
        }

        public Task<List<RegularCustomerDto>> GetRegularCustomersAsync(int minPurchaseCount = 3)
        {
            if (minPurchaseCount < 1)
            {
                throw new InvalidOperationException("minPurchaseCount must be at least 1.");
            }
            return _repo.GetRegularCustomersAsync(minPurchaseCount);
        }

        public Task<List<HighSpenderDto>> GetHighSpendersAsync(int topN = 10)
        {
            if (topN < 1)
            {
                throw new InvalidOperationException("topN must be at least 1.");
            }
            return _repo.GetHighSpendersAsync(topN);
        }

        public Task<List<PendingCreditDto>> GetPendingCreditsAsync() =>
            _repo.GetPendingCreditsAsync();

        private static FinancialReportDto BuildFinancialReport(
            DateTime start,
            DateTime end,
            string periodType,
            List<SalesInvoice> invoices)
        {
            return new FinancialReportDto
            {
                PeriodStart = start,
                PeriodEnd = end,
                PeriodType = periodType,
                InvoiceCount = invoices.Count,
                GrossSales = invoices.Sum(i => i.SubTotal),
                TotalDiscount = invoices.Sum(i => i.DiscountAmount),
                NetSales = invoices.Sum(i => i.TotalAmount),
                TotalPaid = invoices.Sum(i => i.PaidAmount),
                TotalCredit = invoices.Sum(i => i.CreditAmount)
            };
        }
    }
}
