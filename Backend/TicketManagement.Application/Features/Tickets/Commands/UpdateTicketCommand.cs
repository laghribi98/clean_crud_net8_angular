using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Common.Wrappers;
using TicketManagement.Application.Features.Dtos;
using TicketManagement.Domain.Enums;

namespace TicketManagement.Application.Features.Tickets.Commands
{
    public class UpdateTicketCommand : IRequest<Response<UpdateTicketResponse>>
    {
        public int TicketId { get; set; }
        public string Description { get; set; } = String.Empty;
        public string Status { get; set; }
    }
}
