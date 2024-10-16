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
                _logger.LogError($"Une erreur s'est produite: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new Response<string>();

            switch (exception)
            {
                case NotFoundException _:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response = new Response<string>("La ressource demandée est introuvable.");
                    break;

                case ValidationException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response = new Response<string>("Les données fournies ne sont pas valides.");
                    response.Errors = ex.Errors.ToList();
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response = new Response<string>("Une erreur inattendue s'est produite.");
                    response.Errors.Add(exception.Message);
                    break;
            }

            var result = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(result);
        }
    }
}
