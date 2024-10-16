using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Features.Tickets.Queries;

namespace TicketManagement.Application.Features.Tickets.Validators
{
    public class GetTicketByIdQueryValidator : AbstractValidator<GetTicketByIdQuery>
    {
        public GetTicketByIdQueryValidator()
        {
            RuleFor(x => x.TicketId)
                .GreaterThan(0).WithMessage("L'identifiant du ticket doit être un entier positif.");
        }
    }
}
