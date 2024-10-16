using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Application.Common.Wrappers
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public List<string> Errors { get; set; }

        public Response()
        {
            Errors = new List<string>();
        }

        public Response(T data, string message = "")
        {
            Success = true;
            Message = message;
            Data = data;
            Errors = new List<string>();
        }

        public Response(string errorMessage)
        {
            Success = false;
            Message = errorMessage;
            Errors = new List<string> { errorMessage };
        }

        public Response(List<string> errorMessages)
        {
            Success = false;
            Errors = errorMessages;
        }
    }

}
