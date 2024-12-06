using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using Backend.Controllers;  // Backend namespace'ini doğru eklediğinizden emin olun
using Backend.Models;
using Backend.Data;

var builder = WebApplication.CreateBuilder(args);

// CORS ayarlarını yapıyoruz
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Herhangi bir origin'e izin ver
              .AllowAnyMethod()  // Tüm HTTP yöntemlerine (GET, POST, PUT, DELETE) izin ver
              .AllowAnyHeader(); // Herhangi bir header'a izin ver
    });
});

// Veritabanı bağlantısı
var connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
if (string.IsNullOrEmpty(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// CORS yapılandırmasını devreye alıyoruz
app.UseCors("AllowAll");

// Swagger middleware'ini etkinleştiriyoruz
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware'leri kullanıyoruz
app.UseAuthorization();

app.MapGet("/", () => "Hello, Heroku!");

var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://0.0.0.0:{port}");
