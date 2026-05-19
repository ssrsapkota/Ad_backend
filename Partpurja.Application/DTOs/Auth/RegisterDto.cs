using System.ComponentModel.DataAnnotations;

namespace Partpurja.Application.DTOs.Auth
{
    public class RegisterDto
    {
        [Required, MinLength(3), MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string Phone { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        // Optional Vehicle Details for single-operation registration
        public string? VehicleRegistrationNumber { get; set; }
        public string? VehicleBrand { get; set; }
        public string? VehicleModel { get; set; }
        public int? VehicleYear { get; set; }
        public string? VehicleChassisNumber { get; set; }
        public string? VehicleCondition { get; set; }
        public int? MonthlyUsageKm { get; set; }
    }
}
