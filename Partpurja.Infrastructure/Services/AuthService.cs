using Partpurja.Application.DTOs.Auth;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Domain.Models;

namespace Partpurja.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private const string CustomerRoleName = "Customer";

        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<AuthResponseDto> RegisterCustomerAsync(RegisterDto dto)
        {
            if (await _userRepository.ExistsByUsernameAsync(dto.Username))
            {
                return new AuthResponseDto { Success = false, Message = "Username already taken." };
            }

            if (await _userRepository.ExistsByEmailAsync(dto.Email))
            {
                return new AuthResponseDto { Success = false, Message = "Email already registered." };
            }

            var role = await _userRepository.GetRoleByNameAsync(CustomerRoleName)
                       ?? await _userRepository.CreateRoleAsync(CustomerRoleName);

            var user = new User
            {
                Username = dto.Username,
                Email = dto.Email,
                PasswordHash = _passwordHasher.Hash(dto.Password),
                RoleId = role.Id
            };

            var customer = new Customer
            {
                FullName = dto.FullName,
                Phone = dto.Phone,
                Address = dto.Address
            };

            if (!string.IsNullOrWhiteSpace(dto.VehicleRegistrationNumber))
            {
                customer.Vehicles.Add(new Vehicle
                {
                    RegistrationNumber = dto.VehicleRegistrationNumber,
                    Brand = dto.VehicleBrand ?? string.Empty,
                    Model = dto.VehicleModel ?? string.Empty,
                    Year = dto.VehicleYear ?? DateTime.UtcNow.Year,
                    ChassisNumber = dto.VehicleChassisNumber ?? string.Empty,
                    VehicleCondition = dto.VehicleCondition ?? string.Empty,
                    MonthlyUsageKm = dto.MonthlyUsageKm ?? 0,
                    IsActive = true
                });
            }

            var created = await _userRepository.CreateUserWithCustomerAsync(user, customer);

            return new AuthResponseDto
            {
                Success = true,
                UserId = created.Id,
                CustomerId = customer.Id,
                Username = created.Username,
                Email = created.Email,
                Role = role.Name,
                Message = "Registration successful."
            };
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByUsernameAsync(dto.Username);
            if (user == null || !user.IsActive)
            {
                return null;
            }

            if (!_passwordHasher.Verify(dto.Password, user.PasswordHash))
            {
                return null;
            }

            return new AuthResponseDto
            {
                Success = true,
                UserId = user.Id,
                CustomerId = user.Customer?.Id,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role?.Name ?? string.Empty,
                Message = "Login successful."
            };
        }
    }
}
