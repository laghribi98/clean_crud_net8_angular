using FluentValidation.Results;
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
    public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Response<CreateTicketResponse>>
    {
        private readonly ITicketRepository _ticketRepository;

        public CreateTicketCommandHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public async Task<Response<CreateTicketResponse>> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            // Validate the status value
            if (!Enum.IsDefined(typeof(TicketStatus), request.Status))
            {
                throw new ArgumentException("Invalid status provided. The status must be 'Open' or 'Closed'.", nameof(request.Status));
            }

            // Creating the new ticket entity from the request
            var ticket = new Ticket
            {
                Description = request.Description,
                Status = request.Status,
                Date = DateTime.UtcNow
            };

            try
            {
                // Add the new ticket to the repository (which will save it in the database)
                await _ticketRepository.AddAsync(ticket);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while creating the ticket: {ex.Message}", ex);
            }

            // Construct the response DTO
            var response = new CreateTicketResponse
            {
                TicketId = ticket.Ticket_ID,
                Description = ticket.Description,
                Status = ticket.Status.ToString() // Convert enum to string for better readability
            };

            // Return success response with ticket details
            return new Response<CreateTicketResponse>(response, "Ticket created successfully.", status: 201);
        }

        private ValidationResult Validate(CreateTicketCommand request)
        {
            // Simule la validation
            var errors = new List<ValidationFailure>();
            if (string.IsNullOrEmpty(request.Description))
            {
                errors.Add(new ValidationFailure("Description", "La description est requise."));
            }
            return new ValidationResult(errors);
        }
    }
}

