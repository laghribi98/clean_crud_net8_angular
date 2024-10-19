using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TicketManagement.Application.Features.Tickets.Handlers;
using TicketManagement.Infrastructure.Extensions;
using TicketManagement.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using FluentValidation;
using TicketManagement.Application.Features.Tickets.Validators;
using TicketManagement.Api.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Charger la configuration des bases de données
var configuration = builder.Configuration;

// Enregistrer les validateurs à partir de l'assembly contenant CreateTicketCommandValidator
builder.Services.AddValidatorsFromAssemblyContaining<CreateTicketCommandValidator>();

// Enregistrer l'infrastructure, y compris le DbContext
builder.Services.AddInfrastructure(configuration);

// Ajouter MediatR pour gérer les Commands et Queries
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTicketCommandHandler).Assembly));

// Ajouter FluentValidation pour les contrôleurs
builder.Services.AddControllers()
    .AddFluentValidation();  // Active FluentValidation pour valider les entrées de l'API

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configurer Swagger et Endpoints API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
// Enregistrer le middleware de gestion des erreurs
app.UseMiddleware<ExceptionMiddleware>();

// Appliquer les migrations et créer la base de données si elle n'existe pas (seulement en développement)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TicketDbContext>();

    if (app.Environment.IsDevelopment())
    {
        dbContext.Database.Migrate();  // Appliquer les migrations uniquement en développement
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();
app.Run();
