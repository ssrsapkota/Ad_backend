using Microsoft.EntityFrameworkCore;
using Partpurja.Application.DTOs.CreditReminder;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;

namespace Partpurja.Infrastructure.Repository
{
    public class CreditReminderRepository : ICreditReminderRepository
    {
        private readonly AppDbContext _context;

        public CreditReminderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CreditReminderDto>> GetAllAsync()
        {
            var reminders = await _context.CreditReminders
                .Include(r => r.Customer)
                    .ThenInclude(c => c!.User)
                .OrderBy(r => r.DueDate)
                .ToListAsync();

            return reminders.Select(MapToDto);
        }

        public async Task<IEnumerable<SalesInvoice>> GetOverdueInvoicesAsync(int overdueDays)
        {
            var cutoff = DateTime.UtcNow.AddDays(-overdueDays);

            return await _context.SalesInvoices
                .Include(si => si.Customer)
                    .ThenInclude(c => c!.User)
                .Where(si => si.CreditAmount > 0 && si.Date <= cutoff && si.SendCreditReminder)
                .ToListAsync();
        }

        public async Task<bool> ExistsForInvoiceAsync(int salesInvoiceId)
        {
            return await _context.CreditReminders
                .AnyAsync(r => r.SalesInvoiceId == salesInvoiceId);
        }

        public async Task<CreditReminderDto> CreateAsync(int customerId, int salesInvoiceId, decimal amount, DateTime dueDate)
        {
            var reminder = new CreditReminder
            {
                CustomerId = customerId,
                SalesInvoiceId = salesInvoiceId,
                Amount = amount,
                DueDate = dueDate,
                IsEmailSent = false,
                Status = CreditReminderStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            _context.CreditReminders.Add(reminder);
            await _context.SaveChangesAsync();

            // Re-fetch with Customer included so DTO mapping has the data
            var created = await _context.CreditReminders
                .Include(r => r.Customer)
                    .ThenInclude(c => c!.User)
                .FirstAsync(r => r.Id == reminder.Id);

            return MapToDto(created);
        }

        public async Task<bool> MarkEmailSentAsync(int id)
        {
            var reminder = await _context.CreditReminders.FindAsync(id);

            if (reminder == null)
            {
                return false;
            }

            reminder.IsEmailSent = true;
            reminder.Status = CreditReminderStatus.Sent;
            reminder.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        private CreditReminderDto MapToDto(CreditReminder reminder)
        {
            return new CreditReminderDto
            {
                Id = reminder.Id,
                CustomerId = reminder.CustomerId,
                CustomerName = reminder.Customer?.FullName ?? string.Empty,
                CustomerEmail = reminder.Customer?.User?.Email ?? string.Empty,
                SalesInvoiceId = reminder.SalesInvoiceId,
                Amount = reminder.Amount,
                DueDate = reminder.DueDate,
                IsEmailSent = reminder.IsEmailSent,
                Status = reminder.Status,
                CreatedAt = reminder.CreatedAt
            };
        }
    }
}