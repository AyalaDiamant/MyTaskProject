using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace core.Middleware
{
    public record ExceptionResponse(HttpStatusCode StatusCode,string Data);
    public class ErrorHandler
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ErrorHandler> _logger;

        public ErrorHandler(RequestDelegate next,
                     ILogger<ErrorHandler> logger,
                     IWebHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionResponseAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionResponseAsync(HttpContext context, Exception ex)
        {
            var innerException = ex.InnerException != null ? $",Internal Error: {ex.Message},InnerException: {ex.InnerException}" : "";

            ExceptionResponse response = ex switch
            {
                ApplicationException _ => new ExceptionResponse(HttpStatusCode.BadRequest, $"BadRequest: {ex.Message} {innerException}"),
                KeyNotFoundException _ => new ExceptionResponse(HttpStatusCode.NotFound, $"NotFound: {ex.Message} {innerException}"),
                UnauthorizedAccessException _ => new ExceptionResponse(HttpStatusCode.Unauthorized, $"Unauthorized: {ex.Message} {innerException}"),
                _ => new ExceptionResponse(HttpStatusCode.InternalServerError, $"Internal Error: {ex.Message} {innerException}")
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)response.StatusCode;
            await context.Response.WriteAsJsonAsync(response);
        }

    }
}
