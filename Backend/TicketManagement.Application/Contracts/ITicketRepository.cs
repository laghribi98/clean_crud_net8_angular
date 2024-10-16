using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Domain.Entities;

namespace TicketManagement.Application.Contracts
{
    public interface ITicketRepository
    {
        Task<Ticket> GetByIdAsync(int id);      // Récupère un ticket par son ID
        Task<List<Ticket>> GetAllAsync();        // Récupère tous les tickets
        Task AddAsync(Ticket ticket);            // Ajoute un nouveau ticket
        Task UpdateAsync(Ticket ticket);         // Met à jour un ticket existant
        Task DeleteAsync(Ticket ticket);         // Supprime un ticket
    }
}
