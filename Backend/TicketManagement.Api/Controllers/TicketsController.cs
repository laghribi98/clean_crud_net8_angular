using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
            var query = new GetTicketByIdQuery(id);
            var result = await _mediator.Send(query);

            if (result.Success)
                return Ok(result.Data);
            return NotFound(result.Message);
        }

        // POST: api/tickets
        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketCommand command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _mediator.Send(command);
            if (result.Success)
                return CreatedAtAction(nameof(GetTicketById), new { id = result.Data }, result.Data);
            return BadRequest(result.Message);
        }

        // PUT: api/tickets/{id}
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateTicket(int id, [FromBody] UpdateTicketCommand command)
        //{
        //    if (id != command.TicketId)
        //        return BadRequest("L'ID du ticket ne correspond pas à l'ID de la commande.");

        //    var result = await _mediator.Send(command);
        //    if (result.Success)
        //        return NoContent();
        //    return BadRequest(result.Message);
        //}

        // DELETE: api/tickets/{id}
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTicket(int id)
        //{
        //    var command = new DeleteTicketCommand { TicketId = id };
        //    var result = await _mediator.Send(command);

        //    if (result.Success)
        //        return NoContent();
        //    return NotFound(result.Message);
        //}
    }

}
