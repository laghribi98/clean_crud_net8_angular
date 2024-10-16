using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TicketManagement.Domain.Entities;

namespace TicketManagement.Infrastructure.Persistence.EntityTypeConfigurations
{
    public class TicketConfiguration : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasKey(t => t.Ticket_ID);  // Clé primaire
            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(200);  // Contraintes de la base de données
            builder.Property(t => t.Status)
                .HasConversion<string>();  // Conversion Enum -> string en base
        }
    }
}
