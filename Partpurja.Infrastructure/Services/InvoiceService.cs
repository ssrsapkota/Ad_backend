using Partpurja.Application.DTOs.Email;
using Partpurja.Application.DTOs.Invoice;
using Partpurja.Application.DTOs.Loyalty;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Domain.Models;

namespace Partpurja.Infrastructure.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly ISalesInvoiceRepository _invoiceRepo;
        private readonly IPartRepository _partRepo;
        private readonly ILoyaltyService _loyaltyService;
        private readonly IStockMonitorService _stockMonitorService;
        private readonly IEmailService _emailService;

        public InvoiceService(
            ISalesInvoiceRepository invoiceRepo,
            IPartRepository partRepo,
            ILoyaltyService loyaltyService,
            IStockMonitorService stockMonitorService,
            IEmailService emailService)
        {
            _invoiceRepo = invoiceRepo;
            _partRepo = partRepo;
            _loyaltyService = loyaltyService;
            _stockMonitorService = stockMonitorService;
            _emailService = emailService;
        }

        public async Task<InvoiceDto?> GetByIdAsync(int id)
        {
            var invoice = await _invoiceRepo.GetByIdAsync(id);
            return invoice == null ? null : Map(invoice);
        }

        public async Task<List<InvoiceDto>> GetByCustomerIdAsync(int customerId)
        {
            var invoices = await _invoiceRepo.GetByCustomerIdAsync(customerId);
            return invoices.Select(Map).ToList();
        }

        public async Task<InvoiceDto> CreateAsync(CreateInvoiceDto dto)
        {
            if (dto.Items.Count == 0)
            {
                throw new InvalidOperationException("Invoice must contain at least one item.");
            }

            var partIds = dto.Items.Select(i => i.PartId).ToList();
            var parts = await _partRepo.GetByIdsAsync(partIds);
            var partsById = parts.ToDictionary(p => p.Id);

            var items = new List<SalesInvoiceItem>();
            decimal subTotal = 0m;

            foreach (var line in dto.Items)
            {
                if (!partsById.TryGetValue(line.PartId, out var part))
                {
                    throw new InvalidOperationException($"Part with id {line.PartId} not found.");
                }

                if (part.Stock < line.Quantity)
                {
                    throw new InvalidOperationException(
                        $"Insufficient stock for part '{part.Name}'. Available: {part.Stock}, requested: {line.Quantity}.");
                }

                var lineTotal = part.Price * line.Quantity;
                subTotal += lineTotal;

                items.Add(new SalesInvoiceItem
                {
                    PartId = part.Id,
                    Quantity = line.Quantity,
                    UnitPrice = part.Price,
                    TotalPrice = lineTotal
                });

                part.Stock -= line.Quantity;
                part.UpdatedAt = DateTime.UtcNow;
            }

            var loyalty = await _loyaltyService.CalculateAsync(new LoyaltyCalculationRequestDto
            {
                SubTotal = subTotal
            });

            var totalAmount = loyalty.TotalAmount;
            var paidAmount = Math.Min(dto.PaidAmount, totalAmount);
            var creditAmount = Math.Max(0m, totalAmount - paidAmount);

            var status = creditAmount == 0m
                ? InvoiceStatus.Paid
                : paidAmount > 0m ? InvoiceStatus.PartiallyPaid : InvoiceStatus.Pending;

            var invoice = new SalesInvoice
            {
                InvoiceNumber = GenerateInvoiceNumber(),
                CustomerId = dto.CustomerId,
                Date = DateTime.UtcNow,
                SubTotal = subTotal,
                DiscountAmount = loyalty.DiscountAmount,
                TotalAmount = totalAmount,
                PaidAmount = paidAmount,
                CreditAmount = creditAmount,
                Status = status,
                Items = items
            };

            var created = await _invoiceRepo.CreateAsync(invoice);

            // Persist stock changes and check for low-stock alerts.
            await _partRepo.SaveChangesAsync();
            await _stockMonitorService.CheckLowStockAsync();

            var dtoOut = Map(created);
            dtoOut.LoyaltyDiscountApplied = loyalty.IsLoyaltyDiscountApplied;

            foreach (var item in dtoOut.Items)
            {
                if (partsById.TryGetValue(item.PartId, out var part))
                {
                    item.PartName = part.Name;
                }
            }

            return dtoOut;
        }

        public async Task<bool> SendInvoiceEmailAsync(int invoiceId)
        {
            var invoice = await _invoiceRepo.GetByIdWithCustomerAsync(invoiceId);
            if (invoice == null)
            {
                throw new InvalidOperationException($"Invoice with id {invoiceId} not found.");
            }

            var email = invoice.Customer?.User?.Email;
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidOperationException("Customer has no email address on file.");
            }

            var customerName = invoice.Customer?.FullName ?? "Customer";

            var sent = await _emailService.SendEmailAsync(new EmailMessageDto
            {
                To = email,
                ToName = customerName,
                Subject = $"Your Partpurja Invoice #{invoice.InvoiceNumber}",
                Body = BuildInvoiceEmailBody(customerName, invoice)
            });

            if (sent)
            {
                await _invoiceRepo.MarkEmailedAsync(invoice.Id);
            }
            return sent;
        }

        private static string BuildInvoiceEmailBody(string customerName, SalesInvoice invoice)
        {
            var rows = string.Join("", invoice.Items.Select(i =>
                $"<tr><td>{i.Part?.Name ?? "Part"}</td><td>{i.Quantity}</td><td>{i.UnitPrice:N2}</td><td>{i.TotalPrice:N2}</td></tr>"));

            return $"""
                <p>Dear {customerName},</p>
                <p>Thank you for your purchase. Below is your invoice <strong>#{invoice.InvoiceNumber}</strong>
                dated <strong>{invoice.Date:MMMM d, yyyy}</strong>.</p>
                <table border="1" cellpadding="6" cellspacing="0">
                  <thead><tr><th>Part</th><th>Qty</th><th>Unit Price</th><th>Total</th></tr></thead>
                  <tbody>{rows}</tbody>
                </table>
                <p>Subtotal: <strong>Rs. {invoice.SubTotal:N2}</strong><br/>
                Discount: <strong>Rs. {invoice.DiscountAmount:N2}</strong><br/>
                Total: <strong>Rs. {invoice.TotalAmount:N2}</strong><br/>
                Paid: <strong>Rs. {invoice.PaidAmount:N2}</strong><br/>
                Credit Outstanding: <strong>Rs. {invoice.CreditAmount:N2}</strong></p>
                <p>Thank you,<br/>Partpurja Management System</p>
                """;
        }

        private static string GenerateInvoiceNumber()
        {
            return $"INV-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
        }

        private static InvoiceDto Map(SalesInvoice invoice) => new()
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            CustomerId = invoice.CustomerId,
            Date = invoice.Date,
            SubTotal = invoice.SubTotal,
            DiscountAmount = invoice.DiscountAmount,
            LoyaltyDiscountApplied = invoice.DiscountAmount > 0m,
            TotalAmount = invoice.TotalAmount,
            PaidAmount = invoice.PaidAmount,
            CreditAmount = invoice.CreditAmount,
            Status = invoice.Status.ToString(),
            Items = invoice.Items.Select(i => new InvoiceItemDto
            {
                Id = i.Id,
                PartId = i.PartId,
                PartName = i.Part?.Name ?? string.Empty,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                TotalPrice = i.TotalPrice
            }).ToList()
        };
    }
}
