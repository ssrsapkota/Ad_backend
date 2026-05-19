using Microsoft.EntityFrameworkCore;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Repository
{
    public class PartRepository : IPartRepository
    {
        private readonly AppDbContext _context;

        public PartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Part>> GetAllAsync()
        {
            return await _context.Parts
                .Where(p => p.IsActive)
                .OrderBy(p => p.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Part>> GetLowStockPartsAsync()
        {
            return await _context.Parts
                .Where(p => p.IsActive && p.Stock < p.ReorderLevel)
                .OrderBy(p => p.Stock)
                .ToListAsync();
        }

        public async Task<Part?> GetByIdAsync(int id)
        {
            return await _context.Parts.FindAsync(id);
        }

        public async Task<List<Part>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var idList = ids.Distinct().ToList();
            return await _context.Parts
                .Where(p => idList.Contains(p.Id))
                .ToListAsync();
        }

        public async Task<Part?> GetByPartNumberAsync(string partNumber)
        {
            return await _context.Parts.FirstOrDefaultAsync(p => p.PartNumber == partNumber);
        }

        public async Task<Part> CreateAsync(Part part)
        {
            _context.Parts.Add(part);
            await _context.SaveChangesAsync();
            return part;
        }

        public async Task<Part?> UpdateAsync(int id, Part part)
        {
            var existing = await _context.Parts.FindAsync(id);
            if (existing == null) return null;

            existing.PartNumber = part.PartNumber;
            existing.Name = part.Name;
            existing.Category = part.Category;
            existing.Description = part.Description;
            existing.Price = part.Price;
            existing.ReorderLevel = part.ReorderLevel;
            existing.VendorId = part.VendorId;
            existing.IsActive = part.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.Parts.FindAsync(id);
            if (existing == null || !existing.IsActive) return false;

            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public Task SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
