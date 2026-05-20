using Partpurja.Application.DTOs.ServiceReview;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Domain.Models;

namespace Partpurja.Infrastructure.Services
{
    public class ServiceReviewService : IServiceReviewService
    {
        private readonly IServiceReviewRepository _repo;
        private readonly IVehicleRepository _vehicleRepo;

        public ServiceReviewService(IServiceReviewRepository repo, IVehicleRepository vehicleRepo)
        {
            _repo = repo;
            _vehicleRepo = vehicleRepo;
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
            int vehicleId = dto.VehicleId;
            if (vehicleId <= 0)
            {
                var vehicles = await _vehicleRepo.GetByCustomerIdAsync(dto.CustomerId);
                var activeVehicle = vehicles.FirstOrDefault(v => v.IsActive);
                if (activeVehicle != null)
                {
                    vehicleId = activeVehicle.Id;
                }
                else
                {
                    var anyVehicle = vehicles.FirstOrDefault();
                    if (anyVehicle != null)
                    {
                        vehicleId = anyVehicle.Id;
                    }
                    else
                    {
                        // Create a placeholder vehicle
                        var placeholder = new Vehicle
                        {
                            CustomerId = dto.CustomerId,
                            RegistrationNumber = "TEMP-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                            Brand = "Placeholder",
                            Model = "Review Placeholder",
                            Year = DateTime.UtcNow.Year,
                            ChassisNumber = "TEMP-CHASSIS-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
                            VehicleCondition = "Good",
                            MonthlyUsageKm = 0,
                            IsActive = false
                        };
                        var createdPlaceholder = await _vehicleRepo.CreateAsync(placeholder);
                        vehicleId = createdPlaceholder.Id;
                    }
                }
            }

            string comment = string.IsNullOrWhiteSpace(dto.Comment) ? dto.Comments : dto.Comment;

            var review = new ServiceReview
            {
                CustomerId = dto.CustomerId,
                VehicleId = vehicleId,
                Rating = dto.Rating,
                Comment = comment
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