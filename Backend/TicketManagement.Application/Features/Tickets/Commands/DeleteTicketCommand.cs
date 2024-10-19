using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Common.Wrappers;

namespace TicketManagement.Application.Features.Tickets.Commands
{
    public class DeleteTicketCommand : IRequest<Response<string>>
    {
        public int TicketId { get; set; }

        public DeleteTicketCommand(int ticketId)
        {
            TicketId = ticketId;
        }
    }
}
