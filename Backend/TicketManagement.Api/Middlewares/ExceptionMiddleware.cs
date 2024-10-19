using System.Net;
using System.Text.Json;
using TicketManagement.Application.Common.Wrappers;
using TicketManagement.Application.Exceptions;

namespace TicketManagement.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new Response<string>();
            context.Response.StatusCode = exception switch
            {
                ArgumentException _ => (int)HttpStatusCode.BadRequest,
                NotFoundException _ => (int)HttpStatusCode.NotFound,
                ValidationException _ => (int)HttpStatusCode.BadRequest,
                ApplicationException _ => (int)HttpStatusCode.InternalServerError,
                _ => (int)HttpStatusCode.InternalServerError
            };

            response.Status = context.Response.StatusCode;

            response.Message = exception switch
            {
                ArgumentException ex => ex.Message,
                NotFoundException ex => ex.Message,
                ValidationException ex => "Validation error occurred.",
                ApplicationException ex => "An application error occurred.",
                _ => "An unexpected error occurred."
            };

            if (exception is ValidationException validationException)
            {
                response.Errors = new Dictionary<string, List<string>>
                {
                    { "General", validationException.Errors.ToList() }
                };
            }

            var result = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(result);
        }
    }
}

