using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Contracts;
using TicketManagement.Application.Exceptions;
using TicketManagement.Application.Features.Tickets.Commands;
using TicketManagement.Application.Features.Tickets.Handlers;
using TicketManagement.Domain.Entities;
using TicketManagement.Domain.Enums;

namespace TicketManagement.Tests.Features.Tickets.Handlers
{
    public  class CreateTicketCommandHandlerTests
    {
        private readonly Mock<ITicketRepository> _ticketRepositoryMock;
        private readonly CreateTicketCommandHandler _handler;

        public CreateTicketCommandHandlerTests()
        {
            _ticketRepositoryMock = new Mock<ITicketRepository>();
            _handler = new CreateTicketCommandHandler(_ticketRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenValidCommand_ReturnsTicketIdAndSuccessMessage()
        {
            // Arrange
            var command = new CreateTicketCommand
            {
                Description = "New Ticket",
                Status = TicketStatus.Open
            };

            _ticketRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Ticket>())).Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(201, result.Status);
            Assert.Equal("Ticket created successfully.", result.Message);
            Assert.NotNull(result.Data);
            Assert.Equal("New Ticket", result.Data.Description);
            Assert.Equal(TicketStatus.Open.ToString(), result.Data.Status);
        }

        [Fact]
        public async Task Handle_WhenInvalidStatusIsProvided_ThrowsArgumentException()
        {
            // Arrange
            var command = new CreateTicketCommand
            {
                Description = "New Ticket",
                Status = (TicketStatus)999 // Invalid status
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrowsException_ThrowsApplicationException()
        {
            // Arrange
            var command = new CreateTicketCommand
            {
                Description = "New Ticket",
                Status = TicketStatus.Open
            };

            _ticketRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<Ticket>())).ThrowsAsync(new System.Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<ApplicationException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
