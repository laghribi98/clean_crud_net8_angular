using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Common.Wrappers;
using TicketManagement.Application.Contracts;
using TicketManagement.Application.Exceptions;
using TicketManagement.Application.Features.Tickets.Commands;

namespace TicketManagement.Application.Features.Tickets.Handlers
{
    public class DeleteTicketCommandHandler:IRequestHandler<DeleteTicketCommand, Response<string>>
{
    private readonly ITicketRepository _ticketRepository;

    public DeleteTicketCommandHandler(ITicketRepository ticketRepository)
    {
        _ticketRepository = ticketRepository;
    }

        public async Task<Response<string>> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the existing ticket to ensure it exists
            var ticket = await _ticketRepository.GetByIdAsync(request.TicketId);
            if (ticket == null)
            {
                throw new NotFoundException($"The ticket with ID {request.TicketId} was not found.");
            }

            // Delete the ticket from the repository
            await _ticketRepository.DeleteAsync(ticket);

            // Construct a successful response without errors
            var response = new Response<string>
            {
                Status = 200,
                Message = $"Ticket with ID {request.TicketId} was successfully deleted.",
                Data = null,
                Errors = null // Ensure no errors are returned for successful operations
            };

            return response;
        }
    }
}
