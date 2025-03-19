using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Infrastructures.ResponseBuilder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructures.Exceptions
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

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An unhandled exception has occurred: {ex.Message}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            
            var response = new ApiResponse<object>
            {
                Success = false
            };

            switch (exception)
            {
                case ValidationException validationException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = validationException.Message;
                    break;
                case NotFoundException notFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.Message = notFoundException.Message;
                    break;
                case ApiException apiException:
                    context.Response.StatusCode = apiException.StatusCode;
                    response.Message = apiException.Message;
                    response.Data = apiException.ErrorDetails;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.Message = "Internal Server Error. Please try again later.";
                    break;
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            var json = JsonSerializer.Serialize(response, options);
            await context.Response.WriteAsync(json);
        }
    }
} 