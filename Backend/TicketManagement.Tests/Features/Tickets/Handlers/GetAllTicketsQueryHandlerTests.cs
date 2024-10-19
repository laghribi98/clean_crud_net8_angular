using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketManagement.Application.Contracts;
using TicketManagement.Application.Features.Tickets.Handlers;
using TicketManagement.Application.Features.Tickets.Queries;
using TicketManagement.Domain.Entities;
using TicketManagement.Domain.Enums;

namespace TicketManagement.Tests.Features.Tickets.Handlers
{
    public class GetAllTicketsQueryHandlerTests
    {
        private readonly Mock<ITicketRepository> _ticketRepositoryMock;
        private readonly GetAllTicketsQueryHandler _handler;

        public GetAllTicketsQueryHandlerTests()
        {
            _ticketRepositoryMock = new Mock<ITicketRepository>();
            _handler = new GetAllTicketsQueryHandler(_ticketRepositoryMock.Object);
        }

        [Fact]
        public async Task Handle_WhenPageSizeIsZeroOrNegative_UsesDefaultPageSize()
        {
            // Arrange
            var tickets = GetSampleTickets(15); // Generating sample tickets

            _ticketRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tickets);

            var query = new GetAllTicketsQuery(pageNumber: 1, pageSize: 0); // Invalid pageSize

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Status);
            Assert.Equal(10, result.Data.Items.Count); // Default pageSize is 10
            Assert.Equal(2, result.Data.TotalPages);  // 15 tickets, default page size is 10, hence 2 pages
        }

        [Fact]
        public async Task Handle_WhenPageNumberIsZeroOrNegative_UsesDefaultPageNumber()
        {
            // Arrange
            var tickets = GetSampleTickets(10);

            _ticketRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tickets);

            var query = new GetAllTicketsQuery(pageNumber: 0, pageSize: 5); // Invalid pageNumber

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Status);
            Assert.Equal(5, result.Data.Items.Count); // pageSize of 5
            Assert.Equal(1, result.Data.PageNumber);  // Default pageNumber is 1
        }

        [Fact]
        public async Task Handle_WhenDataSetIsEmpty_HandlesGracefully()
        {
            // Arrange
            _ticketRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Ticket>());

            var query = new GetAllTicketsQuery(pageNumber: 1, pageSize: 10);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Status);
            Assert.Empty(result.Data.Items);
            Assert.Equal(1, result.Data.PageNumber);
            Assert.Equal(0, result.Data.TotalCount);
        }

        [Fact]
        public async Task Handle_WhenRequestingLastPage_CorrectlyReturnsRemainingItems()
        {
            // Arrange
            var tickets = GetSampleTickets(15);

            _ticketRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tickets);

            var query = new GetAllTicketsQuery(pageNumber: 2, pageSize: 10); // Requesting the last page

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Status);
            Assert.Equal(5, result.Data.Items.Count); // Should return 5 tickets on the second page
            Assert.Equal(2, result.Data.PageNumber);
            Assert.Equal(15, result.Data.TotalCount);
        }

        [Fact]
        public async Task Handle_WhenRequestingBeyondAvailablePages_ReturnsEmptyResult()
        {
            // Arrange
            var tickets = GetSampleTickets(5);

            _ticketRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tickets);

            var query = new GetAllTicketsQuery(pageNumber: 3, pageSize: 5); // Requesting a page beyond available data

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Status);
            Assert.Empty(result.Data.Items); // No items should be returned
            Assert.Equal(3, result.Data.PageNumber);
            Assert.Equal(5, result.Data.TotalCount);
        }

        [Fact]
        public async Task Handle_WhenLargeDataSetIsQueried_PaginatesCorrectly()
        {
            // Arrange
            var tickets = GetSampleTickets(100); // Simulate a larger data set

            _ticketRepositoryMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(tickets);

            var query = new GetAllTicketsQuery(pageNumber: 4, pageSize: 25); // Page 4, expecting 25 tickets per page

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.Status);
            Assert.Equal(25, result.Data.Items.Count); // Each page should contain 25 items
            Assert.Equal(4, result.Data.PageNumber);
            Assert.Equal(100, result.Data.TotalCount);
            Assert.Equal(4, result.Data.TotalPages); // Total pages should be 4 (100 tickets, page size of 25)
        }

        // Helper method to generate sample tickets
        private List<Ticket> GetSampleTickets(int count)
        {
            var tickets = new List<Ticket>();

            for (int i = 1; i <= count; i++)
            {
                tickets.Add(new Ticket
                {
                    Ticket_ID = i,
                    Description = $"Ticket {i} description",
                    Status = i % 2 == 0 ? TicketStatus.Closed : TicketStatus.Open,
                    Date = System.DateTime.UtcNow
                });
            }

            return tickets;
        }
    }
}

