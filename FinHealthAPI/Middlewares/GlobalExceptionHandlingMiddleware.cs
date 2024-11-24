
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace FinHealthAPI.Middlewares
{
    public class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger _logger;

        public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                var statusCode = (int)HttpStatusCode.InternalServerError;

                context.Response.StatusCode = statusCode;

                ProblemDetails problem = new()
                {
                    Status = statusCode,
                    Type = "Server Error",
                    Title = "Server Error",
                    Detail = "Ah ocurrido un error interno en el servidor"
                };

                string json = JsonSerializer.Serialize(problem);

                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);    

            }
        }
    }
}
