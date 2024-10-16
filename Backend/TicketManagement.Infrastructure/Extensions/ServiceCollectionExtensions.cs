using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Contracts;
using TicketManagement.Infrastructure.Persistence;
using TicketManagement.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;

namespace TicketManagement.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var provider = configuration.GetSection("DatabaseProvider").Value;

            // Enregistrement du DbContext en fonction du fournisseur de base de données
            if (provider == "SqlServer")
            {
                services.AddDbContext<TicketDbContext>(options =>
                    options.UseSqlServer(configuration.GetConnectionString("SqlServerConnection")));
            }
            else if (provider == "InMemory")
            {
                services.AddDbContext<TicketDbContext>(options =>
                    options.UseInMemoryDatabase("InMemoryDb"));
            }

            // Enregistrer les repositories et autres services d'infrastructure
            services.AddScoped<ITicketRepository, TicketRepository>();

            return services;
        }
    }
}
