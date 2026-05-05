using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.DTOs.Vehicle;
using Partpurja.Application.Interface.IServices;

namespace Partpurja_Management_system.Controllers
{
    // Controller for Vehicle Management
    [ApiController]
    [Route("api/vehicles")]
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleService _service;

        public VehiclesController(IVehicleService service)
        {
            _service = service;
        }
        //Get API to get all vehicles
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }
        //Post API to create a new vehicle
        [HttpPost]
        public async Task<IActionResult> Create(CreateVehicleDto dto)
        {
            return Ok(await _service.CreateAsync(dto));
        }
    }
}