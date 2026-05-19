namespace Partpurja.Application.DTOs.Auth
{
    public class AuthResponseDto
    {
        public int UserId { get; set; }
        public int? CustomerId { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
    }
}
