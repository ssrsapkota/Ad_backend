using Partpurja.Application.DTOs.Staff;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;

namespace Partpurja.Infrastructure.Services
{
    public class StaffService : IStaffService
    {
        private readonly IStaffRepository _staffRepository;

        public StaffService(IStaffRepository staffRepository)
        {
            _staffRepository = staffRepository;
        }

        public async Task<IEnumerable<StaffDto>> GetAllStaffAsync()
        {
            return await _staffRepository.GetAllAsync();
        }

        public async Task<StaffDto?> GetStaffByIdAsync(int id)
        {
            return await _staffRepository.GetByIdAsync(id);
        }

        public async Task<StaffDto> CreateStaffAsync(CreateStaffDto dto)
        {
            return await _staffRepository.CreateAsync(dto);
        }

        public async Task<StaffDto?> UpdateStaffAsync(int id, UpdateStaffDto dto)
        {
            return await _staffRepository.UpdateAsync(id, dto);
        }

        public async Task<bool> DeleteStaffAsync(int id)
        {
            return await _staffRepository.DeleteAsync(id);
        }
    }
}
