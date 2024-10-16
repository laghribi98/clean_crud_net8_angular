using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Infrastructure.Persistence
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TicketDbContext>
    {
        public TicketDbContext CreateDbContext(string[] args)
        {
            // Obtenir le répertoire du projet API (là où se trouve appsettings.json)
            var directory = Directory.GetCurrentDirectory();
            var apiProjectPath = Path.Combine(directory, "..", "TicketManagement.Api");

            // Charger la configuration depuis le fichier appsettings.json du projet API
            var configuration = new ConfigurationBuilder()
                .SetBasePath(apiProjectPath)
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<TicketDbContext>();
            var connectionString = configuration.GetConnectionString("SqlServerConnection");

            // Configurer le DbContext pour utiliser SQL Server
            builder.UseSqlServer(connectionString);

            return new TicketDbContext(builder.Options);
        }
    }
}
