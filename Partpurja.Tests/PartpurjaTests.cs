using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Partpurja.Application.DTOs.Email;
using Partpurja.Application.DTOs.Invoice;
using Partpurja.Application.DTOs.Loyalty;
using Partpurja.Application.DTOs.Notification;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Domain.Models;
using Partpurja.Infrastructure.Persistence;
using Partpurja.Infrastructure.Repository;
using Partpurja.Infrastructure.Services;
using Xunit;

namespace Partpurja.Tests
{
    public class PartpurjaTests
    {
        // ==========================================
        // 1. LOYALTY ENGINE DISCOUNT TESTS
        // ==========================================

        [Theory]
        [InlineData(4999.00, false, 0.00, 4999.00)]
        [InlineData(5000.00, false, 0.00, 5000.00)]
        [InlineData(5000.01, true, 500.00, 4500.01)]
        [InlineData(6000.00, true, 600.00, 5400.00)]
        public async Task LoyaltyService_EnforcesStrictlyExceeding5000Threshold(
            decimal subTotal,
            bool expectedDiscountApplied,
            decimal expectedDiscountAmount,
            decimal expectedTotalAmount)
        {
            // Arrange
            var loyaltyService = new LoyaltyService();
            var request = new LoyaltyCalculationRequestDto { SubTotal = subTotal };

            // Act
            var result = await loyaltyService.CalculateAsync(request);

            // Assert
            Assert.Equal(expectedDiscountApplied, result.IsLoyaltyDiscountApplied);
            Assert.Equal(expectedDiscountAmount, result.DiscountAmount);
            Assert.Equal(expectedTotalAmount, result.TotalAmount);
        }

        [Fact]
        public async Task InvoiceService_AppliesLoyaltyDiscountToTotalAmount_WhenExceeding5000()
        {
            // Arrange
            var mockInvoiceRepo = new Mock<ISalesInvoiceRepository>();
            var mockPartRepo = new Mock<IPartRepository>();
            var loyaltyService = new LoyaltyService(); // Use concrete service to verify mathematics
            var mockStockMonitor = new Mock<IStockMonitorService>();
            var mockEmailService = new Mock<IEmailService>();

            var part = new Part
            {
                Id = 1,
                Name = "Alternator",
                Price = 5001m, // Strictly exceeds 5000
                Stock = 10,
                ReorderLevel = 5,
                IsActive = true
            };

            mockPartRepo.Setup(r => r.GetByIdsAsync(It.IsAny<IEnumerable<int>>()))
                .ReturnsAsync(new List<Part> { part });

            mockInvoiceRepo.Setup(r => r.CreateAsync(It.IsAny<SalesInvoice>()))
                .ReturnsAsync((SalesInvoice si) => {
                    si.Id = 123;
                    return si;
                });

            var invoiceService = new InvoiceService(
                mockInvoiceRepo.Object,
                mockPartRepo.Object,
                loyaltyService,
                mockStockMonitor.Object,
                mockEmailService.Object
            );

            var createDto = new CreateInvoiceDto
            {
                CustomerId = 1,
                PaidAmount = 5000m,
                Items = new List<CreateInvoiceItemDto>
                {
                    new CreateInvoiceItemDto { PartId = 1, Quantity = 1 }
                }
            };

            // Act
            var result = await invoiceService.CreateAsync(createDto);

            // Assert
            Assert.True(result.LoyaltyDiscountApplied);
            Assert.Equal(500.10m, result.DiscountAmount); // 10% of 5001
            Assert.Equal(4500.90m, result.TotalAmount);   // 5001 - 500.10
        }

        // ==========================================
        // 2. STOCK DECREASE NOTIFICATION ALERTS
        // ==========================================

        [Fact]
        public async Task StockMonitorService_GeneratesNotifications_WhenStockIs9ButNot11()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new AppDbContext(options))
            {
                // Seed data: one part with stock 9 (below 10), one part with stock 11 (above 10)
                context.Parts.AddRange(
                    new Part { Id = 1, PartNumber = "P9", Name = "Low Stock Part", Stock = 9, ReorderLevel = 10, IsActive = true, VendorId = 1 },
                    new Part { Id = 2, PartNumber = "P11", Name = "High Stock Part", Stock = 11, ReorderLevel = 10, IsActive = true, VendorId = 1 },
                    new Part { Id = 3, PartNumber = "P10", Name = "Exact Boundary Part", Stock = 10, ReorderLevel = 10, IsActive = true, VendorId = 1 }
                );

                // Seed one Admin user to receive the alerts
                var adminRole = new Role { Id = 1, Name = "Admin" };
                context.Roles.Add(adminRole);
                context.Users.Add(new User { Id = 1, Username = "admin_user", RoleId = 1 });

                await context.SaveChangesAsync();
            }

            // Act & Assert
            using (var context = new AppDbContext(options))
            {
                var partRepo = new PartRepository(context);
                var notificationRepo = new NotificationRepository(context);
                var mockLogger = new Mock<ILogger<StockMonitorService>>();

                var stockMonitor = new StockMonitorService(partRepo, notificationRepo, mockLogger.Object);

                // 1. Verify query logic on PartRepository.GetLowStockPartsAsync()
                var lowStockParts = (await partRepo.GetLowStockPartsAsync()).ToList();
                Assert.Single(lowStockParts);
                Assert.Equal(1, lowStockParts[0].Id); // Only low stock part with stock 9 should be queried

                // 2. Run stock check process
                var notificationsCreated = await stockMonitor.CheckLowStockAsync();

                // Assert that only 1 notification was created for the admin user, targeting part 1
                Assert.Equal(1, notificationsCreated);
                
                var notification = await context.Notifications.FirstOrDefaultAsync();
                Assert.NotNull(notification);
                Assert.Equal(1, notification.UserId);
                Assert.Contains("Low stock alert: Low Stock Part", notification.Title);
                Assert.Contains("Current stock: 9", notification.Message);
            }
        }

        // ==========================================
        // 3. CREDIT COLLECTION TRIGGER TESTS
        // ==========================================

        [Fact]
        public async Task CreditReminderService_SendsWarningEmail_OnlyWhenBalanceOverdueMoreThanOneMonth()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using (var context = new AppDbContext(options))
            {
                var customerRole = new Role { Id = 3, Name = "Customer" };
                context.Roles.Add(customerRole);

                var customerUser = new User { Id = 10, Username = "debt_customer", Email = "debt@example.com", RoleId = 3 };
                context.Users.Add(customerUser);

                var customerProfile = new Customer { Id = 5, UserId = 10, FullName = "John Doe", Phone = "987654321", Address = "Kathmandu" };
                context.Customers.Add(customerProfile);

                // 1. Overdue invoice: dated 32 days ago, carrying 2000 credit
                context.SalesInvoices.Add(new SalesInvoice
                {
                    Id = 100,
                    InvoiceNumber = "INV-OLD",
                    CustomerId = 5,
                    Date = DateTime.UtcNow.AddDays(-32), // Older than 1 month
                    SubTotal = 2000m,
                    DiscountAmount = 0m,
                    TotalAmount = 2000m,
                    PaidAmount = 0m,
                    CreditAmount = 2000m,
                    Status = InvoiceStatus.Pending
                });

                // 2. Recent invoice: dated 15 days ago, carrying 1500 credit (should not trigger)
                context.SalesInvoices.Add(new SalesInvoice
                {
                    Id = 101,
                    InvoiceNumber = "INV-NEW",
                    CustomerId = 5,
                    Date = DateTime.UtcNow.AddDays(-15), // Less than 1 month
                    SubTotal = 1500m,
                    DiscountAmount = 0m,
                    TotalAmount = 1500m,
                    PaidAmount = 0m,
                    CreditAmount = 1500m,
                    Status = InvoiceStatus.Pending
                });

                // 3. Paid invoice: dated 40 days ago, no credit outstanding (should not trigger)
                context.SalesInvoices.Add(new SalesInvoice
                {
                    Id = 102,
                    InvoiceNumber = "INV-PAID",
                    CustomerId = 5,
                    Date = DateTime.UtcNow.AddDays(-40),
                    SubTotal = 3000m,
                    DiscountAmount = 0m,
                    TotalAmount = 3000m,
                    PaidAmount = 3000m,
                    CreditAmount = 0m,
                    Status = InvoiceStatus.Paid
                });

                await context.SaveChangesAsync();
            }

            // Act & Assert
            using (var context = new AppDbContext(options))
            {
                var creditRepo = new CreditReminderRepository(context);
                var mockEmailService = new Mock<IEmailService>();
                var mockLogger = new Mock<ILogger<CreditReminderService>>();

                mockEmailService.Setup(e => e.SendEmailAsync(It.IsAny<EmailMessageDto>()))
                    .ReturnsAsync(true);

                var creditReminderService = new CreditReminderService(creditRepo, mockEmailService.Object, mockLogger.Object);

                // Verify that repository queries only the old overdue credit invoice
                var overdueInvoices = (await creditRepo.GetOverdueInvoicesAsync(30)).ToList();
                Assert.Single(overdueInvoices);
                Assert.Equal(100, overdueInvoices[0].Id);

                // Run processing
                var emailsSent = await creditReminderService.ProcessOverdueCreditsAsync();

                // Assert that exactly 1 email was sent (for invoice 100)
                Assert.Equal(1, emailsSent);

                mockEmailService.Verify(e => e.SendEmailAsync(It.Is<EmailMessageDto>(dto =>
                    dto.To == "debt@example.com" &&
                    dto.Subject.Contains("INV-OLD") &&
                    dto.Body.Contains("Rs. 2,000.00")
                )), Times.Once);

                var reminder = await context.CreditReminders.FirstOrDefaultAsync(r => r.SalesInvoiceId == 100);
                Assert.NotNull(reminder);
                Assert.True(reminder.IsEmailSent);
                Assert.Equal(CreditReminderStatus.Sent, reminder.Status);
            }
        }
    }
}
