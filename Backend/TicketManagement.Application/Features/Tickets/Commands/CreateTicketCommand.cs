﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using TicketManagement.Application.Common.Wrappers;
using TicketManagement.Application.Features.Dtos;
using TicketManagement.Domain.Enums;

namespace TicketManagement.Application.Features.Tickets.Commands
{
    public class CreateTicketCommand : IRequest<Response<CreateTicketResponse>>
    {
        public string Description { get; set; } = String.Empty;
        public TicketStatus Status { get; set; }


    }
}
