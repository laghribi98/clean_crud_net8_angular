using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Sockets;
using System;
using System.ComponentModel.DataAnnotations;
using TicketManagement.Domain.Enums;

namespace TicketManagement.Domain.Entities
{
    public class Ticket
    {
        [Key]
        [Required]
        public int Ticket_ID { get; set; }
        public string Description { get; set; }

        public TicketStatus Status { get; set; }

        public DateTime Date { get; set; }

        public Ticket()
        {
            Description = string.Empty;
            Date = DateTime.Today;
            //Status = TicketStatus.Open;
        }

        // Méthode pour mettre à jour la description
        public void UpdateDescription(string newDescription)
        {
            if (string.IsNullOrWhiteSpace(newDescription))
            {
                throw new ArgumentException("La description ne peut pas être vide.");
            }
            Description = newDescription;
        }

        // Méthode pour changer le statut
        public void CloseTicket()
        {
            if (Status == TicketStatus.Closed)
            {
                throw new InvalidOperationException("Le ticket est déjà fermé.");
            }
            Status = TicketStatus.Closed;
        }

        public void ReopenTicket()
        {
            if (Status == TicketStatus.Open)
            {
                throw new InvalidOperationException("Le ticket est déjà ouvert.");
            }
            Status = TicketStatus.Open;
        }

    }
}
