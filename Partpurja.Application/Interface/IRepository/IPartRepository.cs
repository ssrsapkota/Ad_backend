using Partpurja.Domain.Models;

namespace Partpurja.Application.Interface.IRepository
{
    public interface IPartRepository
    {
        Task<IEnumerable<Part>> GetAllAsync();
        Task<IEnumerable<Part>> GetLowStockPartsAsync();
        Task<Part?> GetByIdAsync(int id);
        Task<List<Part>> GetByIdsAsync(IEnumerable<int> ids);
        Task<Part?> GetByPartNumberAsync(string partNumber);
        Task<Part> CreateAsync(Part part);
        Task<Part?> UpdateAsync(int id, Part part);
        Task<bool> DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}
