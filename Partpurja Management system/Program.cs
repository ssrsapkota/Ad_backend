using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Partpurja.Application.DTOs.Customers;
using Partpurja.Application.Interface.IRepository;
using Partpurja.Application.Interface.IServices;
using Partpurja.Application.Services;
using Partpurja.Infrastructure.Presistance;
using Partpurja.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

// OpenAPI (no Swagger UI)
builder.Services.AddOpenApi();

builder.Services.AddControllers();

// CORS (optional; safe to keep)
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllowFrontend", p =>
        p.WithOrigins("http://localhost:5173")
         .AllowAnyHeader()
         .AllowAnyMethod());
});

// DB provider from config
var provider = builder.Configuration["Database:Provider"] ?? "Postgres";

builder.Services.AddDbContext<AppDbContext>(opt =>
{
    if (string.Equals(provider, "Postgres", StringComparison.OrdinalIgnoreCase))
        opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    else
        opt.UseInMemoryDatabase("partpurja_dev");
});

// DI for Feature 6
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ISearchService, SearchService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // /openapi/v1.json (for Postman import)
    app.MapOpenApi();
}

// Optional helpers
app.MapGet("/", () => Results.Ok("Partpurja API running"));
app.MapGet("/health", () => Results.Ok("Healthy"));

app.UseCors("AllowFrontend");

app.MapControllers();

// Apply migrations (Postgres) or EnsureCreated (InMemory)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (string.Equals(provider, "Postgres", StringComparison.OrdinalIgnoreCase))
        db.Database.Migrate();
    else
        db.Database.EnsureCreated();
}



// Print URLs for easy copy to Postman
app.Lifetime.ApplicationStarted.Register(() =>
{
    var urls = string.Join(", ", app.Urls);
    Console.WriteLine("Now listening on: " + urls);
});

app.Run();