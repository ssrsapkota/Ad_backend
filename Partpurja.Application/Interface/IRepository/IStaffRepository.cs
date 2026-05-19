using Partpurja.Application.DTOs.Staff;

namespace Partpurja.Application.Interface.IRepository
{
    public interface IStaffRepository
    {
        Task<IEnumerable<StaffDto>> GetAllAsync();
        Task<StaffDto?> GetByIdAsync(int id);
        Task<StaffDto?> UpdateAsync(int id, UpdateStaffDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
