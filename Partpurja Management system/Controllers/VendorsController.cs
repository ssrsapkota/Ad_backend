using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.DTOs.Vendor;
using Partpurja.Application.Interface.IServices;

namespace Partpurja.Api.Controllers
{
    /// <summary>
    /// API controller for vendor management operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class VendorsController : ControllerBase
    {
        private readonly IVendorService _vendorService;

        public VendorsController(IVendorService vendorService)
        {
            _vendorService = vendorService;
        }

        /// <summary>
        /// Gets all active vendors.
        /// </summary>
        /// <returns>A list of all active vendors.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VendorDto>>> GetAllVendors()
        {
            var vendors = await _vendorService.GetAllVendorsAsync();
            return Ok(vendors);
        }

        /// <summary>
        /// Gets a vendor by ID.
        /// </summary>
        /// <param name="id">The vendor ID.</param>
        /// <returns>The vendor if found; otherwise NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<VendorDto>> GetVendorById(int id)
        {
            var vendor = await _vendorService.GetVendorByIdAsync(id);

            if (vendor == null)
            {
                return NotFound(new { message = "Vendor not found" });
            }

            return Ok(vendor);
        }

        /// <summary>
        /// Creates a new vendor.
        /// </summary>
        /// <param name="createVendorDto">The vendor data to create.</param>
        /// <returns>The created vendor with 201 status code.</returns>
        [HttpPost]
        public async Task<ActionResult<VendorDto>> CreateVendor([FromBody] CreateVendorDto createVendorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vendor = await _vendorService.CreateVendorAsync(createVendorDto);
            return CreatedAtAction(nameof(GetVendorById), new { id = vendor.Id }, vendor);
        }

        /// <summary>
        /// Updates an existing vendor.
        /// </summary>
        /// <param name="id">The vendor ID.</param>
        /// <param name="updateVendorDto">The vendor data to update.</param>
        /// <returns>The updated vendor if found; otherwise NotFound.</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<VendorDto>> UpdateVendor(int id, [FromBody] UpdateVendorDto updateVendorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vendor = await _vendorService.UpdateVendorAsync(id, updateVendorDto);

            if (vendor == null)
            {
                return NotFound(new { message = "Vendor not found" });
            }

            return Ok(vendor);
        }

        /// <summary>
        /// Soft deletes a vendor.
        /// </summary>
        /// <param name="id">The vendor ID.</param>
        /// <returns>NoContent if successful; otherwise NotFound.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVendor(int id)
        {
            var success = await _vendorService.DeleteVendorAsync(id);

            if (!success)
            {
                return NotFound(new { message = "Vendor not found" });
            }

            return NoContent();
        }
    }
}
