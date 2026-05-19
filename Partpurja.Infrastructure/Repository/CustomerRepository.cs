using Microsoft.EntityFrameworkCore;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;

        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        public async Task<Customer?> GetByUserIdAsync(int userId)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer?> UpdateAsync(int id, Customer customer)
        {
            var existing = await _context.Customers.FindAsync(id);
            if (existing == null) return null;

            existing.FullName = customer.FullName;
            existing.Phone = customer.Phone;
            existing.Address = customer.Address;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<List<Customer>> SearchAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return new List<Customer>();
            }

            var trimmed = query.Trim();
            var lowered = trimmed.ToLower();
            var customers = _context.Customers
                .Include(c => c.User)
                .Include(c => c.Vehicles)
                .Where(c => c.IsActive);

            if (int.TryParse(trimmed, out var idCandidate))
            {
                // Numeric query — match by Customer.Id OR phone (phone numbers are stored as strings).
                customers = customers.Where(c =>
                    c.Id == idCandidate ||
                    c.Phone.Contains(trimmed));
            }
            else
            {
                customers = customers.Where(c =>
                    c.FullName.ToLower().Contains(lowered) ||
                    c.Phone.Contains(trimmed) ||
                    c.Vehicles.Any(v => v.RegistrationNumber.ToLower().Contains(lowered)));
            }

            return await customers.OrderBy(c => c.FullName).ToListAsync();
        }
    }
}
