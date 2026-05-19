using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.DTOs.Staff;
using Partpurja.Application.Interface.IServices;

namespace Partpurja.Api.Controllers
{
    /// <summary>
    /// API controller for staff management operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly IStaffService _staffService;

        public StaffController(IStaffService staffService)
        {
            _staffService = staffService;
        }

        /// <summary>
        /// Gets all staff members.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StaffDto>>> GetAllStaff()
        {
            var staff = await _staffService.GetAllStaffAsync();
            return Ok(staff);
        }

        /// <summary>
        /// Gets a staff member by ID.
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<StaffDto>> GetStaffById(int id)
        {
            var staff = await _staffService.GetStaffByIdAsync(id);

            if (staff == null)
            {
                return NotFound(new { message = "Staff not found" });
            }

            return Ok(staff);
        }

        /// <summary>
        /// Creates a new staff member.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<StaffDto>> CreateStaff([FromBody] CreateStaffDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var staff = await _staffService.CreateStaffAsync(dto);
                return CreatedAtAction(nameof(GetStaffById), new { id = staff.Id }, staff);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Updates an existing staff member.
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<StaffDto>> UpdateStaff(int id, [FromBody] UpdateStaffDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var staff = await _staffService.UpdateStaffAsync(id, dto);

            if (staff == null)
            {
                return NotFound(new { message = "Staff not found" });
            }

            return Ok(staff);
        }

        /// <summary>
        /// Deletes a staff member.
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStaff(int id)
        {
            var success = await _staffService.DeleteStaffAsync(id);

            if (!success)
            {
                return NotFound(new { message = "Staff not found" });
            }

            return NoContent();
        }
    }
}