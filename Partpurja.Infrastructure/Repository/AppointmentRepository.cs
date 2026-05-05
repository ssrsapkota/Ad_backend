using Microsoft.EntityFrameworkCore;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly AppDbContext _context;

    public AppointmentRepository(AppDbContext context)
    {
        _context = context;
    }
    // Method to get all appointments for a specific customer
    public async Task<List<Appointment>> GetByCustomerIdAsync(int customerId)
    {
        return await _context.Appointments
            .Where(x => x.CustomerId == customerId)
            .ToListAsync();
    }
}