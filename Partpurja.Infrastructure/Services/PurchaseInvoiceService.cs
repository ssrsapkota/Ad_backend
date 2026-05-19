using Partpurja.Application.DTOs.PurchaseInvoice;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Domain.Models;

namespace Partpurja.Infrastructure.Services
{
    public class PurchaseInvoiceService : IPurchaseInvoiceService
    {
        private readonly IPurchaseInvoiceRepository _purchaseRepo;
        private readonly IPartRepository _partRepo;

        public PurchaseInvoiceService(
            IPurchaseInvoiceRepository purchaseRepo,
            IPartRepository partRepo)
        {
            _purchaseRepo = purchaseRepo;
            _partRepo = partRepo;
        }

        public async Task<List<PurchaseInvoiceDto>> GetAllAsync()
        {
            var invoices = await _purchaseRepo.GetAllAsync();
            return invoices.Select(Map).ToList();
        }

        public async Task<PurchaseInvoiceDto?> GetByIdAsync(int id)
        {
            var invoice = await _purchaseRepo.GetByIdAsync(id);
            return invoice == null ? null : Map(invoice);
        }

        public async Task<PurchaseInvoiceDto> CreateAsync(CreatePurchaseInvoiceDto dto)
        {
            if (dto.Items.Count == 0)
            {
                throw new InvalidOperationException("Purchase invoice must contain at least one item.");
            }

            var partIds = dto.Items.Select(i => i.PartId).ToList();
            var parts = await _partRepo.GetByIdsAsync(partIds);
            var partsById = parts.ToDictionary(p => p.Id);

            var items = new List<PurchaseInvoiceItem>();
            decimal totalBeforeDiscount = 0m;

            foreach (var line in dto.Items)
            {
                if (!partsById.TryGetValue(line.PartId, out var part))
                {
                    throw new InvalidOperationException($"Part with id {line.PartId} not found.");
                }

                var lineTotal = line.UnitPrice * line.Quantity;
                totalBeforeDiscount += lineTotal;

                items.Add(new PurchaseInvoiceItem
                {
                    PartId = part.Id,
                    Quantity = line.Quantity,
                    UnitPrice = line.UnitPrice,
                    TotalPrice = lineTotal
                });

                // Trigger: incoming stock increases inventory
                part.Stock += line.Quantity;
                part.UpdatedAt = DateTime.UtcNow;
            }

            if (dto.DiscountAmount > totalBeforeDiscount)
            {
                throw new InvalidOperationException("DiscountAmount cannot exceed the invoice subtotal.");
            }

            var totalAmount = totalBeforeDiscount - dto.DiscountAmount;
            var paidAmount = Math.Min(dto.PaidAmount, totalAmount);
            var status = paidAmount >= totalAmount
                ? InvoiceStatus.Paid
                : paidAmount > 0m ? InvoiceStatus.PartiallyPaid : InvoiceStatus.Pending;

            var invoice = new PurchaseInvoice
            {
                InvoiceNumber = GenerateInvoiceNumber(),
                VendorId = dto.VendorId,
                Date = DateTime.UtcNow,
                TotalAmount = totalAmount,
                PaidAmount = paidAmount,
                DiscountAmount = dto.DiscountAmount,
                Status = status,
                Items = items
            };

            var created = await _purchaseRepo.CreateAsync(invoice);
            // Persist stock changes from the same DbContext (PartRepository wraps the same context).
            await _partRepo.SaveChangesAsync();

            var result = Map(created);
            foreach (var item in result.Items)
            {
                if (partsById.TryGetValue(item.PartId, out var part))
                {
                    item.PartName = part.Name;
                }
            }
            return result;
        }

        private static string GenerateInvoiceNumber()
        {
            return $"PINV-{DateTime.UtcNow:yyyyMMddHHmmss}-{Random.Shared.Next(1000, 9999)}";
        }

        private static PurchaseInvoiceDto Map(PurchaseInvoice invoice) => new()
        {
            Id = invoice.Id,
            InvoiceNumber = invoice.InvoiceNumber,
            VendorId = invoice.VendorId,
            VendorName = invoice.Vendor?.Name ?? string.Empty,
            Date = invoice.Date,
            TotalAmount = invoice.TotalAmount,
            PaidAmount = invoice.PaidAmount,
            DiscountAmount = invoice.DiscountAmount,
            Status = invoice.Status.ToString(),
            Items = invoice.Items.Select(i => new PurchaseInvoiceItemDto
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
