using Partpurja.Application.DTOs.UnavailablePartRequest;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Domain.Models;

namespace Partpurja.Infrastructure.Services
{
    public class UnavailablePartRequestService : IUnavailablePartRequestService
    {
        private readonly IUnavailablePartRequestRepository _repo;

        public UnavailablePartRequestService(IUnavailablePartRequestRepository repo)
        {
            _repo = repo;
        }

        // Get all requests
        public async Task<List<UnavailablePartRequestDto>> GetAllAsync()
        {
            var requests = await _repo.GetAllAsync();

            return requests.Select(r => new UnavailablePartRequestDto
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                RequestedPartName = r.RequestedPartName,
                Quantity = r.Quantity,
                Notes = r.Notes,
                Status = r.Status.ToString(),
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        // Get requests by customer
        public async Task<List<UnavailablePartRequestDto>> GetByCustomerIdAsync(int customerId)
        {
            var requests = await _repo.GetByCustomerIdAsync(customerId);

            return requests.Select(r => new UnavailablePartRequestDto
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                RequestedPartName = r.RequestedPartName,
                Quantity = r.Quantity,
                Notes = r.Notes,
                Status = r.Status.ToString(),
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        // Create request
        public async Task<UnavailablePartRequestDto> CreateAsync(CreateUnavailablePartRequestDto dto)
        {
            var request = new UnavailablePartRequest
            {
                CustomerId = dto.CustomerId,
                RequestedPartName = dto.RequestedPartName,
                Quantity = dto.Quantity,
                Notes = dto.Notes
            };

            var created = await _repo.CreateAsync(request);

            return new UnavailablePartRequestDto
            {
                Id = created.Id,
                CustomerId = created.CustomerId,
                RequestedPartName = created.RequestedPartName,
                Quantity = created.Quantity,
                Notes = created.Notes,
                Status = created.Status.ToString(),
                CreatedAt = created.CreatedAt
            };
        }
    }
}