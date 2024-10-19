﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Features.Tickets.Commands;

namespace TicketManagement.Application.Features.Tickets.Validators
{
    public class UpdateTicketCommandValidator : AbstractValidator<UpdateTicketCommand>
    {
        public UpdateTicketCommandValidator() {
            RuleFor(x => x.TicketId)
                .NotNull().WithMessage("TicketID is required and not null")
                .GreaterThan(0).WithMessage("Ticket ID must be greater than zero."); ;

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("La description est requise.")
                .MaximumLength(200).WithMessage("La description ne peut pas dépasser 200 caractères.");

            RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Le statut doit être soit 'Open' (1) soit 'Closed' (2).");
        }
    }
}
