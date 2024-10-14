using Microsoft.EntityFrameworkCore;
using TicketManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
// Charger la configuration des bases de données
var configuration = builder.Configuration;
var provider = configuration.GetValue<string>("DatabaseProvider");

if (provider == "SqlServer")
{
    builder.Services.AddDbContext<TicketDbContext>(options =>
        options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection")));
}
else if (provider == "InMemory")
{
    builder.Services.AddDbContext<TicketDbContext>(options =>
        options.UseInMemoryDatabase(configuration.GetConnectionString("InMemoryConnection")));
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Configure In-Memory Database

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
