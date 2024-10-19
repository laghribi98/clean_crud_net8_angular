using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketManagement.Application.Common.Wrappers
{
    public class Response<T>
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }

        public Response()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        public Response(T data, string message = null, int status = 200)
        {
            Status = status;
            Message = message;
            Data = data;
            Errors = new Dictionary<string, List<string>>();
        }

        public Response(string errorMessage, int status = 400)
        {
            Status = status;
            Message = errorMessage;
            Errors = new Dictionary<string, List<string>>
        {
            { "General", new List<string> { errorMessage } }
        };
        }

        public Response(Dictionary<string, List<string>> errorMessages, int status = 400)
        {
            Status = status;
            Errors = errorMessages;
        }
    }



}
