using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Common.Wrappers;
using TicketManagement.Application.Contracts;
using TicketManagement.Application.Features.Dtos;
using TicketManagement.Application.Features.Tickets.Queries;

namespace TicketManagement.Application.Features.Tickets.Handlers
{
    public class GetAllTicketsQueryHandler:IRequestHandler<GetAllTicketsQuery, Response<PaginatedList<TicketDto>>>
    {
        private readonly ITicketRepository _ticketRepository;

        public GetAllTicketsQueryHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Response<PaginatedList<TicketDto>>> Handle(GetAllTicketsQuery request, CancellationToken cancellationToken)
        {
            // Retrieve all tickets from the repository
            var tickets = await _ticketRepository.GetAllAsync();

            // Calculate pagination
            var totalTickets = tickets.Count;
            var paginatedTickets = tickets
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Map each Ticket entity to TicketDto
            var ticketDtos = paginatedTickets.Select(ticket => new TicketDto
            {
                TicketId = ticket.Ticket_ID,
                Description = ticket.Description,
                Status = ticket.Status.ToString(), // Convert status enum to string for readability
                CreatedDate = ticket.Date
            }).ToList();

            // Create a PaginatedList to return
            var paginatedList = new PaginatedList<TicketDto>(ticketDtos, totalTickets, request.PageNumber, request.PageSize);

            // Return a successful response with the paginated list of tickets
            return new Response<PaginatedList<TicketDto>>(paginatedList, "Tickets retrieved successfully.", status: 200);
        }
    }
}
