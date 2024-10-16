using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TicketManagement.Application.Common.Wrappers;
using TicketManagement.Domain.Enums;

namespace TicketManagement.Application.Features.Tickets.Commands
{
    public class CreateTicketCommand : IRequest<Response<int>>
    {
        public string Description { get; set; } = String.Empty;
        public TicketStatus Status { get; set; }

        public int TicketId{ get; private set; }
        public DateTime CreatedDate { get; private set; } = DateTime.Now;


    }
}
