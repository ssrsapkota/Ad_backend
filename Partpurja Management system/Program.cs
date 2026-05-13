using Microsoft.EntityFrameworkCore;
using Partpurja.Application.DTOs.Email;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Infrastructure.Persistence;
using Partpurja.Infrastructure.Repository;
using Partpurja.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


// Vendor services
builder.Services.AddScoped<IVendorRepository, VendorRepository>();
builder.Services.AddScoped<IVendorService, VendorService>();

// Customer services
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Vehicle services
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<IVehicleService, VehicleService>();

// Feature 15: Low Stock & Credit Reminders
builder.Services.Configure<EmailSettings>(
builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddScoped<IPartRepository, PartRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<ICreditReminderRepository, CreditReminderRepository>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IStockMonitorService, StockMonitorService>();
builder.Services.AddScoped<ICreditReminderService, CreditReminderService>();

// History services
builder.Services.AddScoped<ISalesInvoiceRepository, SalesInvoiceRepository>();
builder.Services.AddScoped<IHistoryService, HistoryService>();

// Appointment services
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<IUnavailablePartRequestRepository, UnavailablePartRequestRepository>();
builder.Services.AddScoped<IUnavailablePartRequestService, UnavailablePartRequestService>();

// Feature 16: Loyalty Program
builder.Services.AddScoped<ILoyaltyService, LoyaltyService>();

// OpenAPI / Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
