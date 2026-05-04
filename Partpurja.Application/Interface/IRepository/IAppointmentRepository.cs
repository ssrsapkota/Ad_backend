using Partpurja.Domain.Models;

public interface IAppointmentRepository
{
    // Method to get all appointments for a customer
    Task<List<Appointment>> GetByCustomerIdAsync(int customerId);
}