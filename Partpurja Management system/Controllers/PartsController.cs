using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.DTOs.Part;
using Partpurja.Application.Interface.IServices;

namespace Partpurja_Management_system.Controllers
{
    [ApiController]
    [Route("api/parts")]
    public class PartsController : ControllerBase
    {
        private readonly IPartService _service;

        public PartsController(IPartService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var part = await _service.GetByIdAsync(id);
            if (part == null) return NotFound(new { message = "Part not found." });
            return Ok(part);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePartDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdatePartDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _service.UpdateAsync(id, dto);
            if (updated == null) return NotFound(new { message = "Part not found." });
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound(new { message = "Part not found." });
            return NoContent();
        }
    }
}
