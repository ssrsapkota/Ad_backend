using Microsoft.EntityFrameworkCore;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

public class SalesInvoiceRepository : ISalesInvoiceRepository
{
    private readonly AppDbContext _context;

    public SalesInvoiceRepository(AppDbContext context)
    {
        _context = context;
    }
    // Method to get all sales invoices for a specific customer
    public async Task<List<SalesInvoice>> GetByCustomerIdAsync(int customerId)
    {
        return await _context.SalesInvoices
        // To filter sales invoices based on customer ID
            .Where(x => x.CustomerId == customerId)
            .ToListAsync();
    }
}  