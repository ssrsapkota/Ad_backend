using Partpurja.Application.DTOs.Staff;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Domain.Models;

namespace Partpurja.Infrastructure.Services
{
    public class StaffService : IStaffService
    {
        private const string StaffRoleName = "Staff";

        private readonly IStaffRepository _staffRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public StaffService(
            IStaffRepository staffRepository,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher)
        {
            _staffRepository = staffRepository;
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
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
            if (await _userRepository.ExistsByUsernameAsync(dto.Username))
            {
                throw new InvalidOperationException("Username already taken.");
            }

            if (await _userRepository.ExistsByEmailAsync(dto.Email))
            {
                throw new InvalidOperationException("Email already registered.");
            }

            var role = await _userRepository.GetRoleByNameAsync(StaffRoleName)
                       ?? await _userRepository.CreateRoleAsync(StaffRoleName);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = _passwordHasher.Hash(dto.Password),
                RoleId = role.Id
            };

            var staffProfile = new StaffProfile
            {
                FullName = dto.FullName,
                Phone = dto.Phone,
                Position = dto.Position,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.CreateUserWithStaffProfileAsync(user, staffProfile);

            return new StaffDto
            {
                Id = staffProfile.Id,
                UserId = user.Id,
                FullName = staffProfile.FullName,
                Phone = staffProfile.Phone,
                Position = staffProfile.Position,
                IsActive = staffProfile.IsActive,
                CreatedAt = staffProfile.CreatedAt
            };
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
