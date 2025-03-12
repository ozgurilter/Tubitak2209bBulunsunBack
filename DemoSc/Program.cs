using Microsoft.EntityFrameworkCore;
using DemoSc.Data;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://0.0.0.0:5000");

// CORS Politikasý Ýsmi
string myCorsPolicy = "AllowAllOrigins";

// Add Controllers
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);
// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS Servisi
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myCorsPolicy, policy =>
    {
        policy.AllowAnyOrigin() 
              .AllowAnyHeader() 
              .AllowAnyMethod(); 
    });
});

var app = builder.Build();

// Swagger (Development Modunda Çalýþtýrma)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS Kullanýmý
app.UseCors(myCorsPolicy); 

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.Run();
