using Partpurja.Application.DTOs.Appointment;

namespace Partpurja.Application.Interface.IServices
{
    public interface IAppointmentService
    {
        // Get all appointments
        Task<List<AppointmentDto>> GetAllAsync();

        // Get appointments by customer
        Task<List<AppointmentDto>> GetByCustomerIdAsync(int customerId);

        // Create appointment
        Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto);

        // Update appointment status (Pending / Confirmed / Completed / Cancelled)
        Task<AppointmentDto?> UpdateStatusAsync(int id, string status);
    }
}