using Microsoft.EntityFrameworkCore;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Repository
{
    public class SalesInvoiceRepository : ISalesInvoiceRepository
    {
        private readonly AppDbContext _context;

        public SalesInvoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<SalesInvoice>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.SalesInvoices
                .Include(x => x.Items)
                .ThenInclude(i => i.Part)
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<SalesInvoice?> GetByIdAsync(int id)
        {
            return await _context.SalesInvoices
                .Include(x => x.Items)
                .ThenInclude(i => i.Part)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<SalesInvoice?> GetByIdWithCustomerAsync(int id)
        {
            return await _context.SalesInvoices
                .Include(x => x.Items)
                    .ThenInclude(i => i.Part)
                .Include(x => x.Customer)
                    .ThenInclude(c => c!.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<SalesInvoice> CreateAsync(SalesInvoice invoice)
        {
            _context.SalesInvoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<bool> MarkEmailedAsync(int id)
        {
            var invoice = await _context.SalesInvoices.FindAsync(id);
            if (invoice == null) return false;

            invoice.IsInvoiceEmailed = true;
            invoice.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
