using Microsoft.EntityFrameworkCore;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Repository
{
    // Repository implementation for Customer
    public class CustomerRepository : ICustomerRepository
    {
        private readonly AppDbContext _context;
        //Constructor 
        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        // Method to get all customers
        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        // Method to get a customer by ID
        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _context.Customers.FindAsync(id);
        }

        // Method to create a new customer
        public async Task<Customer> CreateAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        // Method to update an existing customer
        public async Task<Customer?> UpdateAsync(int id, Customer customer)
        {
            //To check if the customer exists in the database
            var existing = await _context.Customers.FindAsync(id);
            if (existing == null) return null;

            existing.FullName = customer.FullName;
            existing.Phone = customer.Phone;
            existing.Address = customer.Address;

            await _context.SaveChangesAsync();
            return existing;
        }
    }
}