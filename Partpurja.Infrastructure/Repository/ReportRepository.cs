using Microsoft.EntityFrameworkCore;
using Partpurja.Application.DTOs.Report;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Repository
{
    public class ReportRepository : IReportRepository
    {
        private readonly AppDbContext _context;

        public ReportRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SalesInvoice>> GetSalesInvoicesInRangeAsync(DateTime startInclusive, DateTime endExclusive)
        {
            return await _context.SalesInvoices
                .Where(s => s.Date >= startInclusive && s.Date < endExclusive)
                .ToListAsync();
        }

        public async Task<List<Part>> GetAllPartsAsync()
        {
            return await _context.Parts
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<List<RegularCustomerDto>> GetRegularCustomersAsync(int minPurchaseCount)
        {
            var grouped = await _context.SalesInvoices
                .GroupBy(s => s.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    Count = g.Count()
                })
                .Where(x => x.Count >= minPurchaseCount)
                .ToListAsync();

            var ids = grouped.Select(g => g.CustomerId).ToList();
            var customers = await _context.Customers
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();

            return grouped
                .Select(g =>
                {
                    var c = customers.FirstOrDefault(x => x.Id == g.CustomerId);
                    return new RegularCustomerDto
                    {
                        CustomerId = g.CustomerId,
                        FullName = c?.FullName ?? string.Empty,
                        Phone = c?.Phone ?? string.Empty,
                        PurchaseCount = g.Count
                    };
                })
                .OrderByDescending(r => r.PurchaseCount)
                .ToList();
        }

        public async Task<List<HighSpenderDto>> GetHighSpendersAsync(int topN)
        {
            var grouped = await _context.SalesInvoices
                .GroupBy(s => s.CustomerId)
                .Select(g => new
                {
                    CustomerId = g.Key,
                    Total = g.Sum(x => x.TotalAmount)
                })
                .OrderByDescending(x => x.Total)
                .Take(topN)
                .ToListAsync();

            var ids = grouped.Select(g => g.CustomerId).ToList();
            var customers = await _context.Customers
                .Where(c => ids.Contains(c.Id))
                .ToListAsync();

            return grouped
                .Select(g =>
                {
                    var c = customers.FirstOrDefault(x => x.Id == g.CustomerId);
                    return new HighSpenderDto
                    {
                        CustomerId = g.CustomerId,
                        FullName = c?.FullName ?? string.Empty,
                        Phone = c?.Phone ?? string.Empty,
                        TotalSpent = g.Total
                    };
                })
                .ToList();
        }

        public async Task<List<PendingCreditDto>> GetPendingCreditsAsync()
        {
            var invoices = await _context.SalesInvoices
                .Include(s => s.Customer)
                    .ThenInclude(c => c!.User)
                .Where(s => s.CreditAmount > 0)
                .ToListAsync();

            return invoices
                .GroupBy(s => s.CustomerId)
                .Select(g => new PendingCreditDto
                {
                    CustomerId = g.Key,
                    FullName = g.First().Customer?.FullName ?? string.Empty,
                    Phone = g.First().Customer?.Phone ?? string.Empty,
                    Email = g.First().Customer?.User?.Email ?? string.Empty,
                    OutstandingCredit = g.Sum(x => x.CreditAmount),
                    InvoiceCount = g.Count(),
                    OldestInvoiceDate = g.Min(x => x.Date)
                })
                .OrderByDescending(p => p.OutstandingCredit)
                .ToList();
        }
    }
}
