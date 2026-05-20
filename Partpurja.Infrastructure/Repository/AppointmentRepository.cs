using Microsoft.EntityFrameworkCore;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppDbContext _context;

        public AppointmentRepository(AppDbContext context)
        {
            _context = context;
        }

        // Get all appointments
        public async Task<List<Appointment>> GetAllAsync()
        {
            return await _context.Appointments.ToListAsync();
        }

        // Get appointments by customer
        public async Task<List<Appointment>> GetByCustomerIdAsync(int customerId)
        {
            return await _context.Appointments
                .Where(x => x.CustomerId == customerId)
                .ToListAsync();
        }

        // Create appointment
        public async Task<Appointment> CreateAsync(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return appointment;
        }

        // Get appointment by id
        public async Task<Appointment?> GetByIdAsync(int id)
        {
            return await _context.Appointments.FirstOrDefaultAsync(x => x.Id == id);
        }

        // Update appointment
        public async Task<Appointment> UpdateAsync(Appointment appointment)
        {
            appointment.UpdatedAt = DateTime.UtcNow;
            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }
    }
}