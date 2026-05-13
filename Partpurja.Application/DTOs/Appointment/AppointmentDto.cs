namespace Partpurja.Application.DTOs.Appointment
{
    public class AppointmentDto
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public int VehicleId { get; set; }

        public DateTime AppointmentDate { get; set; }

        public string ServiceDescription { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
}