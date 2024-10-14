using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Domain;

namespace TicketManagement.Infrastructure.Persistence
{
    public class TicketDbContext : DbContext
    {
        public DbSet<Ticket> Tickets { get; set; }
    }
}
