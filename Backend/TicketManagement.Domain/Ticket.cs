using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Sockets;
using System;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.Domain
{
    public class Ticket
    {
        [Key]
        [Required]
        public int Ticket_ID { get; }
        public string Description { get; set; }

        public Status Status { get; set; }

        public DateTime Date { get; set; }

        public Ticket() {
            this.Description = string.Empty;
            this.Date  = DateTime.Today;
            this.Status = Status.Open;
        }

    }
    public enum Status
    {
        Open,
        Close     
    }
}
