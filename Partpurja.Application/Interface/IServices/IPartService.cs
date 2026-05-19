using Partpurja.Application.DTOs.Part;

namespace Partpurja.Application.Interface.IServices
{
    public interface IPartService
    {
        Task<IEnumerable<PartDto>> GetAllAsync();
        Task<PartDto?> GetByIdAsync(int id);
        Task<PartDto> CreateAsync(CreatePartDto dto);
        Task<PartDto?> UpdateAsync(int id, UpdatePartDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
