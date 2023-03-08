using Application.Common.Excentions;
using Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

namespace WebAPI._1_Startup
{
    public static class CustomExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(context, exception);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            switch (exception)
            {
                case BadRequestException badRequestException:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    context.Response.AddArgumentnExcention(exception.Message);
                    break;

                case ForbiddenAccessException forbiddenAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                    context.Response.AddForbiddenExcention(exception.Message);
                    break;

                case UnauthorizedAccessException unauthorizedAccessException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.AddUnauthorisedExcention(exception.Message);
                    break;

                case NotFoundException notFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    context.Response.AddNotFoundExcention(exception.Message);
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.AddApplicationExcention(exception.Message);
                    break;
            }

            await context.Response.WriteAsync(exception.Message);
        }
    }
}