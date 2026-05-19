using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Partpurja.Application.DTOs.Invoice;
using Partpurja.Application.Interface.IServices;

namespace Partpurja_Management_system.Controllers.Invoice
{
    [ApiController]
    [Route("api/invoices")]
    [Authorize]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "Admin,Staff,Customer")]
        public async Task<IActionResult> GetById(int id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);
            if (invoice == null) return NotFound(new { message = "Invoice not found." });
            return Ok(invoice);
        }

        [HttpGet("customer/{customerId:int}")]
        [Authorize(Roles = "Admin,Staff,Customer")]
        public async Task<IActionResult> GetByCustomerId(int customerId)
        {
            var invoices = await _invoiceService.GetByCustomerIdAsync(customerId);
            return Ok(invoices);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> Create([FromBody] CreateInvoiceDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var invoice = await _invoiceService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = invoice.Id }, invoice);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Emails an existing sales invoice to the customer on file.
        /// </summary>
        [HttpPost("{id:int}/email")]
        [Authorize(Roles = "Admin,Staff")]
        public async Task<IActionResult> SendEmail(int id)
        {
            try
            {
                var sent = await _invoiceService.SendInvoiceEmailAsync(id);
                if (!sent)
                {
                    return StatusCode(502, new { message = "Failed to send invoice email." });
                }
                return Ok(new { message = "Invoice email sent." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
