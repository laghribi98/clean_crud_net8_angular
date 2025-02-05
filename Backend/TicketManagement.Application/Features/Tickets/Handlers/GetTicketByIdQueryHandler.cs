﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Common.Wrappers;
using TicketManagement.Application.Contracts;
using TicketManagement.Application.Exceptions;
using TicketManagement.Application.Features.Dtos;
using TicketManagement.Application.Features.Tickets.Queries;

namespace TicketManagement.Application.Features.Tickets.Handlers
{
    public class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, Response<TicketDto>>
    {
        private readonly ITicketRepository _ticketRepository;

        public GetTicketByIdQueryHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Response<TicketDto>> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetByIdAsync(request.TicketId);

            // If ticket is not found, throw NotFoundException
            if (ticket == null)
            {
                throw new NotFoundException($"The ticket with ID {request.TicketId} was not found.");
            }

            var ticketDto = new TicketDto
            {
                TicketId = ticket.Ticket_ID,
                Description = ticket.Description,
                Status = ticket.Status.ToString(),  // Convert to string
                CreatedDate = ticket.Date
            };

            return new Response<TicketDto>(ticketDto, "Ticket retrieved successfully.", status: 200);
        }
    }
}
