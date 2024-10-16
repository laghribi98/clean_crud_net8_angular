using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Common.Wrappers;
using TicketManagement.Application.Features.Dtos;
using TicketManagement.Domain.Entities;

namespace TicketManagement.Application.Features.Tickets.Queries
{
    public class GetTicketByIdQuery : IRequest<Response<TicketDto>>
    {
        public int TicketId { get; set; }

    public GetTicketByIdQuery(int ticketId)
    {
        TicketId = ticketId;
    }
}
}
