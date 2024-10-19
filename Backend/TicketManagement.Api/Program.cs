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

// Charger la configuration des bases de donn�es
var configuration = builder.Configuration;

// Enregistrer les validateurs � partir de l'assembly contenant CreateTicketCommandValidator
builder.Services.AddValidatorsFromAssemblyContaining<CreateTicketCommandValidator>();

// Enregistrer l'infrastructure, y compris le DbContext
builder.Services.AddInfrastructure(configuration);

// Ajouter MediatR pour g�rer les Commands et Queries
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateTicketCommandHandler).Assembly));

// Ajouter FluentValidation pour les contr�leurs
builder.Services.AddControllers()
    .AddFluentValidation();  // Active FluentValidation pour valider les entr�es de l'API

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

// Appliquer les migrations et cr�er la base de donn�es si elle n'existe pas (seulement en d�veloppement)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TicketDbContext>();

    if (app.Environment.IsDevelopment())
    {
        dbContext.Database.Migrate();  // Appliquer les migrations uniquement en d�veloppement
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
