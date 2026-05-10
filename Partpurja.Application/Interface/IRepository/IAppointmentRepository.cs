using Partpurja.Domain.Models;

namespace Partpurja.Application.Interface.IRepository
{
    public interface IAppointmentRepository
    {
        // Get appointments by customer
        Task<List<Appointment>> GetByCustomerIdAsync(int customerId);

        // Get all appointments
        Task<List<Appointment>> GetAllAsync();

        // Create appointment
        Task<Appointment> CreateAsync(Appointment appointment);
    }
}