using System.ComponentModel.DataAnnotations;

namespace Partpurja.Application.DTOs.Vehicle
{
    public class UpdateVehicleDto
    {
        [Required, MaxLength(50)]
        public string RegistrationNumber { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Brand { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Model { get; set; } = string.Empty;

        [Range(1900, 2100)]
        public int Year { get; set; }

        [MaxLength(50)]
        public string ChassisNumber { get; set; } = string.Empty;

        [MaxLength(50)]
        public string VehicleCondition { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int MonthlyUsageKm { get; set; }
    }
}
