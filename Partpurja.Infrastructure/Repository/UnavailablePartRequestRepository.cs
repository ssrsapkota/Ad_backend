using Microsoft.EntityFrameworkCore;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Repository
{
    public class UnavailablePartRequestRepository : IUnavailablePartRequestRepository
    {
        private readonly AppDbContext _context;

        public UnavailablePartRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all requests
        public async Task<List<UnavailablePartRequest>> GetAllAsync()
        {
            return await _context.UnavailablePartRequests.ToListAsync();
        }

        // Get requests by customer
        public async Task<List<UnavailablePartRequest>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.UnavailablePartRequests
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();
        }

        // Create request
        public async Task<UnavailablePartRequest> CreateAsync(UnavailablePartRequest request)
        {
            _context.UnavailablePartRequests.Add(request);
            await _context.SaveChangesAsync();
            return request;
        }
    }
}