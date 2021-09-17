using System;
using System.Net;
using System.Threading.Tasks;
using FluentValidation;
using InstagramClone.Application;
using InstagramClone.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace InstagramClone.Api.CustonMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong : {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ErrorDetails content;

            switch (exception)
            {
                case ApiException apiException:
                    content = new ErrorDetails();
                    content.StatusCode = (int)apiException.StatusCode;
                    content.Message = apiException.Message;
                    break;
                case ValidationException validationException:
                    content = new ErrorDetails
                    {
                        StatusCode = (int)HttpStatusCode.BadRequest,
                        Message = validationException.Message
                    };
                    break;
                default:
                    content = new ErrorDetails
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError,
                        Message = "Internal Server Error"
                    };
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = content.StatusCode;
            return context.Response.WriteAsync(content.ToString());
        }
    }
}