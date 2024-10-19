using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketManagement.Application.Common.Wrappers;
using TicketManagement.Application.Features.Tickets.Commands;
using TicketManagement.Application.Features.Tickets.Queries;

namespace TicketManagement.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TicketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/tickets/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicketById(int id)
        {
            // Use MediatR to send GetTicketByIdQuery to the handler
            var result = await _mediator.Send(new GetTicketByIdQuery(id));

            if (result.Status == 200)
            {
                return Ok(result);
            }

            return StatusCode(result.Status, result);
        }

        // POST: api/tickets
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketCommand command)
        {
            if (!ModelState.IsValid)
            {
                var errors = new Dictionary<string, List<string>>();
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    if (state.Errors.Count > 0)
                    {
                        errors[key] = state.Errors.Select(e => e.ErrorMessage).ToList();
                    }
                }

                return BadRequest(new Response<object>(errors, status: 400));
            }

            var result = await _mediator.Send(command);

            if (result.Status == 201)
            {
                return StatusCode(result.Status, result);
            }

            return StatusCode(result.Status, result);
        }

        // PUT: api/tickets/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTicket(int id, [FromBody] UpdateTicketCommand command)
        {
            if (id != command.TicketId)
            {
                return BadRequest(new Response<string>("The ticket ID in the route does not match the ticket ID in the body.", status: 400));
            }

            // Use MediatR to send the UpdateTicketCommand to the handler
            var result = await _mediator.Send(command);

            if (result.Status == 200)
            {
                return Ok(result);
            }

            return StatusCode(result.Status, result);
        }

        // DELETE: api/tickets/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            // Create a DeleteTicketCommand with the ticket ID
            var command = new DeleteTicketCommand(id);

            // Use MediatR to send the DeleteTicketCommand to the handler
            var result = await _mediator.Send(command);

            if (result.Status == 200)
            {
                return Ok(result);
            }

            return StatusCode(result.Status, result);
        }

        // GET: api/tickets
        [HttpGet]
        public async Task<IActionResult> GetAllTickets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            // Create a GetAllTicketsQuery with pagination parameters
            var query = new GetAllTicketsQuery(pageNumber, pageSize);

            // Use MediatR to send the GetAllTicketsQuery to the handler
            var result = await _mediator.Send(query);

            if (result.Status == 200)
            {
                return Ok(result);
            }

            return StatusCode(result.Status, result);
        }
    }

}
