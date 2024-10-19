using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Common.Wrappers;
using TicketManagement.Application.Features.Dtos;

namespace TicketManagement.Application.Features.Tickets.Queries
{
    public class GetAllTicketsQuery : IRequest<Response<PaginatedList<TicketDto>>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public GetAllTicketsQuery(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < 1 ? 1 : pageNumber;  // Default to 1 if invalid
            PageSize = pageSize < 1 ? 10 : pageSize;        // Default to 10 if invalid
        }
    }
}
