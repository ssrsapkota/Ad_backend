using Microsoft.EntityFrameworkCore;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Repository
{
    public class ServiceReviewRepository : IServiceReviewRepository
    {
        private readonly AppDbContext _context;

        public ServiceReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all reviews
        public async Task<List<ServiceReview>> GetAllAsync()
        {
            return await _context.ServiceReviews.ToListAsync();
        }

        // Get reviews by customer
        public async Task<List<ServiceReview>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.ServiceReviews
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();
        }

        // Create review
        public async Task<ServiceReview> CreateAsync(ServiceReview review)
        {
            _context.ServiceReviews.Add(review);
            await _context.SaveChangesAsync();

            return review;
        }
    }
}