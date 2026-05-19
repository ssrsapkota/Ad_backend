using Microsoft.EntityFrameworkCore;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Repository
{
    public class PurchaseInvoiceRepository : IPurchaseInvoiceRepository
    {
        private readonly AppDbContext _context;

        public PurchaseInvoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<PurchaseInvoice>> GetAllAsync()
        {
            return await _context.PurchaseInvoices
                .Include(p => p.Vendor)
                .Include(p => p.Items)
                    .ThenInclude(i => i.Part)
                .OrderByDescending(p => p.Date)
                .ToListAsync();
        }

        public async Task<PurchaseInvoice?> GetByIdAsync(int id)
        {
            return await _context.PurchaseInvoices
                .Include(p => p.Vendor)
                .Include(p => p.Items)
                    .ThenInclude(i => i.Part)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PurchaseInvoice> CreateAsync(PurchaseInvoice invoice)
        {
            _context.PurchaseInvoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }
    }
}
