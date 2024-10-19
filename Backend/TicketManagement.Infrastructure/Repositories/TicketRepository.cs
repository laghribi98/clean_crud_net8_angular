using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Contracts;
using TicketManagement.Application.Exceptions;
using TicketManagement.Domain.Entities;
using TicketManagement.Infrastructure.Persistence;

namespace TicketManagement.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly TicketDbContext _context;
        public TicketRepository(TicketDbContext context) {
            _context = context;
        }

        public async Task AddAsync(Ticket ticket)
        {
            await _context.Tickets.AddAsync(ticket);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Ticket ticket)
        {
            _context.Tickets.Update(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Ticket ticket)
        {
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Ticket>> GetAllAsync()
        {
            return await _context.Tickets.ToListAsync();
        }

        public async Task<Ticket> GetByIdAsync(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                throw new NotFoundException($"No ticket with ID {id} found.");
            }

            return ticket;
        }

      
    }
}
