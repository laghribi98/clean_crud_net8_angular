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
using TicketManagement.Application.Features.Tickets.Commands;
using TicketManagement.Domain.Entities;

namespace TicketManagement.Application.Features.Tickets.Handlers
{
    public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, Response<int>>
    {
        private readonly ITicketRepository _ticketRepository;

        public CreateTicketCommandHandler(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        async Task<Response<int>> IRequestHandler<CreateTicketCommand, Response<int>>.Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            try
            {
                ValidationResult validationResult = Validate(request);
                if (!validationResult.IsValid)
                {
                    throw new ValidationException(validationResult.Errors);
                }

                var ticket = new Ticket
                {
                    //l'ID est généré automatiquement par la base de données
                    Description = request.Description,
                    Status = request.Status,
                    Date = DateTime.UtcNow
                };

                await _ticketRepository.AddAsync(ticket);

                // Supposez que l'ID du ticket est assigné après l'insertion dans la base de données
                return new Response<int>(ticket.Ticket_ID, "Le ticket a été créé avec succès.");
            }
            catch (Exception ex)
            {
                return new Response<int>("Erreur lors de la création du ticket : " + ex.Message);
            }
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

