using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Domain.Enums;

namespace TicketManagement.Application.Features.Dtos
{
    public class CreateTicketResponse
    {
        public int TicketId { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
    }
}
