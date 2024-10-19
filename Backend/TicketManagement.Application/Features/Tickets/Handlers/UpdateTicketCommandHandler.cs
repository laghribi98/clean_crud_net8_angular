using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Common.Wrappers;
using TicketManagement.Application.Contracts;
using TicketManagement.Application.Exceptions;
using TicketManagement.Application.Features.Dtos;
using TicketManagement.Application.Features.Tickets.Commands;
using TicketManagement.Domain.Entities;
using TicketManagement.Domain.Enums;

namespace TicketManagement.Application.Features.Tickets.Handlers
{
    public class UpdateTicketCommandHandler : IRequestHandler<UpdateTicketCommand, Response<UpdateTicketResponse>>
    {
        private readonly ITicketRepository _ticketRepository;

        public UpdateTicketCommandHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Response<UpdateTicketResponse>> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the existing ticket
            var ticket = await _ticketRepository.GetByIdAsync(request.TicketId);

            if (ticket == null)
            {
                throw new NotFoundException($"The ticket with ID {request.TicketId} was not found.");
            }

            // Validate the status value
            if (!Enum.IsDefined(typeof(TicketStatus), request.Status))
            {
                throw new ArgumentException("Invalid status provided. The status must be 'Open' or 'Closed'.", nameof(request.Status));
            }

            // Update the ticket details
            ticket.Description = request.Description;
            //ticket.Status = request.Status.ToString();

            try
            {
                // Update the ticket in the repository (which will save the changes in the database)
                await _ticketRepository.UpdateAsync(ticket);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while updating the ticket: {ex.Message}", ex);
            }

            // Construct the response DTO
            var response = new UpdateTicketResponse
            {
                TicketId = ticket.Ticket_ID,
                Description = ticket.Description,
                Status = ticket.Status.ToString() // Convert enum to string for better readability
            };

            // Return success response with updated ticket details
            return new Response<UpdateTicketResponse>(response, "Ticket updated successfully.", status: 200);
        }
    }
}
