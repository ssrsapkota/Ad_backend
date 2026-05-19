using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Partpurja.Application.DTOs.Email;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Infrastructure.Persistence;
using Partpurja.Infrastructure.Repository;
using Partpurja.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure CORS for frontend access
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Database connection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Authentication — securely configure JwtBearer with TokenValidationParameters using Jwt:Key, Jwt:Issuer, Jwt:Audience configuration keys.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? string.Empty))
    };
});


// Auth services (customer self-registration & login)
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

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
builder.Services.AddScoped<IServiceReviewRepository, ServiceReviewRepository>();
builder.Services.AddScoped<IServiceReviewService, ServiceReviewService>();

// Staff services
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IStaffService, StaffService>();

// Feature 16: Loyalty Program
builder.Services.AddScoped<ILoyaltyService, LoyaltyService>();

// Sales Invoice service (applies loyalty discount + triggers stock alert)
// (ISalesInvoiceRepository is registered above in "History services")
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

// Part inventory CRUD (admin)
builder.Services.AddScoped<IPartService, PartService>();

// Purchase Invoice flow (admin — stock increment trigger)
builder.Services.AddScoped<IPurchaseInvoiceRepository, PurchaseInvoiceRepository>();
builder.Services.AddScoped<IPurchaseInvoiceService, PurchaseInvoiceService>();

// Reports (admin financial + customer-focused)
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<IReportService, ReportService>();

// OpenAPI / Swagger
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // /openapi/v1.json (for Postman import)
    app.MapOpenApi();
}



app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
