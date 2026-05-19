using Partpurja.Application.DTOs.Auth;

namespace Partpurja.Application.Interface.IServices
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterCustomerAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    }
}
