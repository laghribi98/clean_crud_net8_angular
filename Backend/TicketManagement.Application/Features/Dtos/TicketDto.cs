using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Domain.Enums;

namespace TicketManagement.Application.Features.Dtos
{
    public class TicketDto
    {
        public int TicketId { get; set; }  // Identifiant unique du ticket
        public string? Description { get; set; }  // Description du ticket
        public string Status { get; set; }  // Statut du ticket (ex. "Ouvert", "Fermé")
        public DateTime CreatedDate { get; set; }  // Date de création du ticket
    }
}
