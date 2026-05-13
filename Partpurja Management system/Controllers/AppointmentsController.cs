using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.DTOs.Appointment;
using Partpurja.Application.Interface.IServices;

namespace Partpurja_Management_system.Controllers
{
    [ApiController]
    [Route("api/appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _service;

        public AppointmentsController(IAppointmentService service)
        {
            _service = service;
        }

        // Get all appointments
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var appointments = await _service.GetAllAsync();
            return Ok(appointments);
        }

        // Get appointments by customer
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetByCustomerId(int customerId)
        {
            var appointments = await _service.GetByCustomerIdAsync(customerId);
            return Ok(appointments);
        }

        // Create appointment
        [HttpPost]
        public async Task<IActionResult> Create(CreateAppointmentDto dto)
        {
            var appointment = await _service.CreateAsync(dto);
            return Ok(appointment);
        }
    }
}