using Microsoft.EntityFrameworkCore;
using Partpurja.Application.DTOs.Staff;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Repository
{
    public class StaffRepository : IStaffRepository
    {
        private readonly AppDbContext _context;

        public StaffRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<StaffDto>> GetAllAsync()
        {
            var staff = await _context.StaffProfiles
                .Where(s => s.IsActive)
                .ToListAsync();

            return staff.Select(MapToDto);
        }

        public async Task<StaffDto?> GetByIdAsync(int id)
        {
            var staff = await _context.StaffProfiles.FindAsync(id);

            if (staff == null || !staff.IsActive)
                return null;

            return MapToDto(staff);
        }

        public async Task<StaffDto?> UpdateAsync(int id, UpdateStaffDto dto)
        {
            var staff = await _context.StaffProfiles.FindAsync(id);

            if (staff == null || !staff.IsActive)
                return null;

            staff.FullName = dto.FullName;
            staff.Phone = dto.Phone;
            staff.Position = dto.Position;
            staff.IsActive = dto.IsActive;
            staff.UpdatedAt = DateTime.UtcNow;

            _context.StaffProfiles.Update(staff);
            await _context.SaveChangesAsync();

            return MapToDto(staff);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var staff = await _context.StaffProfiles.FindAsync(id);

            if (staff == null || !staff.IsActive)
                return false;

            staff.IsActive = false;
            staff.UpdatedAt = DateTime.UtcNow;
            _context.StaffProfiles.Update(staff);
            await _context.SaveChangesAsync();

            return true;
        }

        private static StaffDto MapToDto(StaffProfile staff)
        {
            return new StaffDto
            {
                Id = staff.Id,
                UserId = staff.UserId,
                FullName = staff.FullName,
                Phone = staff.Phone,
                Position = staff.Position,
                IsActive = staff.IsActive,
                CreatedAt = staff.CreatedAt
            };
        }
    }
}
