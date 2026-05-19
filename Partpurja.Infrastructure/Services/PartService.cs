using Partpurja.Application.DTOs.Part;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Domain.Models;

namespace Partpurja.Infrastructure.Services
{
    public class PartService : IPartService
    {
        private readonly IPartRepository _repo;

        public PartService(IPartRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<PartDto>> GetAllAsync()
        {
            var parts = await _repo.GetAllAsync();
            return parts.Select(Map);
        }

        public async Task<PartDto?> GetByIdAsync(int id)
        {
            var part = await _repo.GetByIdAsync(id);
            return part == null ? null : Map(part);
        }

        public async Task<PartDto> CreateAsync(CreatePartDto dto)
        {
            var existing = await _repo.GetByPartNumberAsync(dto.PartNumber);
            if (existing != null)
            {
                throw new InvalidOperationException($"Part number '{dto.PartNumber}' is already in use.");
            }

            var part = new Part
            {
                PartNumber = dto.PartNumber,
                Name = dto.Name,
                Category = dto.Category,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                ReorderLevel = dto.ReorderLevel,
                VendorId = dto.VendorId,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repo.CreateAsync(part);
            return Map(created);
        }

        public async Task<PartDto?> UpdateAsync(int id, UpdatePartDto dto)
        {
            var part = new Part
            {
                PartNumber = dto.PartNumber,
                Name = dto.Name,
                Category = dto.Category,
                Description = dto.Description,
                Price = dto.Price,
                ReorderLevel = dto.ReorderLevel,
                VendorId = dto.VendorId,
                IsActive = dto.IsActive
            };

            var updated = await _repo.UpdateAsync(id, part);
            return updated == null ? null : Map(updated);
        }

        public Task<bool> DeleteAsync(int id) => _repo.DeleteAsync(id);

        private static PartDto Map(Part p) => new()
        {
            Id = p.Id,
            PartNumber = p.PartNumber,
            Name = p.Name,
            Category = p.Category,
            Description = p.Description,
            Price = p.Price,
            Stock = p.Stock,
            ReorderLevel = p.ReorderLevel,
            VendorId = p.VendorId,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt
        };
    }
}
