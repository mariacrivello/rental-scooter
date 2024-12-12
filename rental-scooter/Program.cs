using Microsoft.EntityFrameworkCore;
using rental_scooter.Models;
using rental_scooter.Repositories;
using rental_scooter.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x =>
   x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                            builder.Configuration.GetConnectionString("DefaultConnection") ??
                            builder.Configuration.GetConnectionString("AWS");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IRentalHistoryRepository, RentalHistoryRepository>();
builder.Services.AddScoped<IStationRepository, StationRepository>();
builder.Services.AddScoped<IScooterRepository, ScooterRepository>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
