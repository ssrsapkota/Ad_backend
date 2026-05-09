using Microsoft.EntityFrameworkCore;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models.Users;
using Partpurja.Infrastructure.Presistance;

namespace Partpurja.Infrastructure.Repository;

public class CustomerRepository : ICustomerRepository
{
    private readonly AppDbContext _db;
    public CustomerRepository(AppDbContext db) => _db = db;

    public async Task AddAsync(Customer customer, CancellationToken ct = default)
    {
        _db.Customers.Add(customer);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Customer customer, CancellationToken ct = default)
    {
        _db.Customers.Update(customer);
        await _db.SaveChangesAsync(ct);
    }

    public async Task<Customer?> GetByPhoneNumberAsync(string phoneNumber, CancellationToken ct = default)
    {
        return await _db.Customers
            .Include(c => c.Vehicles)
            .FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber, ct);
    }

    public async Task<IEnumerable<Customer>> SearchAsync(string? searchTerm, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await _db.Customers.Include(c => c.Vehicles).ToListAsync(ct);
        }

        searchTerm = searchTerm.ToLower();

        return await _db.Customers
            .Include(c => c.Vehicles)
            .Where(c => c.FirstName.ToLower().Contains(searchTerm) ||
                        c.LastName.ToLower().Contains(searchTerm) ||
                        c.PhoneNumber.Contains(searchTerm) ||
                        c.Id.ToString().Contains(searchTerm) ||
                        c.Vehicles.Any(v => v.VehicleNumber.ToLower().Contains(searchTerm)))
            .ToListAsync(ct);
    }
}