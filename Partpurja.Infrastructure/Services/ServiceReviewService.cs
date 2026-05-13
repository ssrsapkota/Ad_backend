using Partpurja.Application.DTOs.ServiceReview;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Domain.Models;

namespace Partpurja.Infrastructure.Services
{
    public class ServiceReviewService : IServiceReviewService
    {
        private readonly IServiceReviewRepository _repo;

        public ServiceReviewService(IServiceReviewRepository repo)
        {
            _repo = repo;
        }

        // Get all reviews
        public async Task<List<ServiceReviewDto>> GetAllAsync()
        {
            var reviews = await _repo.GetAllAsync();

            return reviews.Select(r => new ServiceReviewDto
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                VehicleId = r.VehicleId,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        // Get reviews by customer
        public async Task<List<ServiceReviewDto>> GetByCustomerIdAsync(int customerId)
        {
            var reviews = await _repo.GetByCustomerIdAsync(customerId);

            return reviews.Select(r => new ServiceReviewDto
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                VehicleId = r.VehicleId,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        // Create review
        public async Task<ServiceReviewDto> CreateAsync(CreateServiceReviewDto dto)
        {
            var review = new ServiceReview
            {
                CustomerId = dto.CustomerId,
                VehicleId = dto.VehicleId,
                Rating = dto.Rating,
                Comment = dto.Comment
            };

            var created = await _repo.CreateAsync(review);

            return new ServiceReviewDto
            {
                Id = created.Id,
                CustomerId = created.CustomerId,
                VehicleId = created.VehicleId,
                Rating = created.Rating,
                Comment = created.Comment,
                CreatedAt = created.CreatedAt
            };
        }
    }
}