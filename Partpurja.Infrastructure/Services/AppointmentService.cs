using Partpurja.Application.DTOs.Appointment;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Domain.Models;

namespace Partpurja.Infrastructure.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repo;

        public AppointmentService(IAppointmentRepository repo)
        {
            _repo = repo;
        }

        // Get all appointments
        public async Task<List<AppointmentDto>> GetAllAsync()
        {
            var appointments = await _repo.GetAllAsync();

            return appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                CustomerId = a.CustomerId,
                VehicleId = a.VehicleId,
                AppointmentDate = a.AppointmentDate,
                ServiceDescription = a.ServiceDescription,
                Status = a.Status.ToString()
            }).ToList();
        }

        // Get appointments by customer
        public async Task<List<AppointmentDto>> GetByCustomerIdAsync(int customerId)
        {
            var appointments = await _repo.GetByCustomerIdAsync(customerId);

            return appointments.Select(a => new AppointmentDto
            {
                Id = a.Id,
                CustomerId = a.CustomerId,
                VehicleId = a.VehicleId,
                AppointmentDate = a.AppointmentDate,
                ServiceDescription = a.ServiceDescription,
                Status = a.Status.ToString()
            }).ToList();
        }

        // Create appointment
        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto)
        {
            var appointment = new Appointment
            {
                CustomerId = dto.CustomerId,
                VehicleId = dto.VehicleId,
                AppointmentDate = dto.AppointmentDate.ToUniversalTime(),
                ServiceDescription = dto.ServiceDescription
            };

            var created = await _repo.CreateAsync(appointment);

            return new AppointmentDto
            {
                Id = created.Id,
                CustomerId = created.CustomerId,
                VehicleId = created.VehicleId,
                AppointmentDate = created.AppointmentDate,
                ServiceDescription = created.ServiceDescription,
                Status = created.Status.ToString()
            };
        }

        // Update appointment status (Pending / Confirmed / Completed / Cancelled)
        public async Task<AppointmentDto?> UpdateStatusAsync(int id, string status)
        {
            if (!Enum.TryParse<AppointmentStatus>(status, ignoreCase: true, out var parsed))
                throw new ArgumentException($"Invalid status '{status}'. Allowed: Pending, Confirmed, Completed, Cancelled.");

            var appointment = await _repo.GetByIdAsync(id);
            if (appointment is null) return null;

            appointment.Status = parsed;
            var updated = await _repo.UpdateAsync(appointment);

            return new AppointmentDto
            {
                Id = updated.Id,
                CustomerId = updated.CustomerId,
                VehicleId = updated.VehicleId,
                AppointmentDate = updated.AppointmentDate,
                ServiceDescription = updated.ServiceDescription,
                Status = updated.Status.ToString()
            };
        }
    }
}