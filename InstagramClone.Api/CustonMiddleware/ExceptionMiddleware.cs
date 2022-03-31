using System;
using System.Linq;
using System.Threading.Tasks;
using InstagramClone.Domain.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InstagramClone.Api.CustonMiddleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next,
            ILogger<ExceptionMiddleware> logger,
            IWebHostEnvironment env)
        {
            _logger = logger;
            _next = next;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred: {ex}");
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            ErrorDetails content;

            switch (exception)
            {
                case ValidationApiException validationApiException:
                    {
                        content = new ValidationErrorDetails()
                        {
                            StatusCode = StatusCodes.Status400BadRequest,
                            Message = validationApiException.Message,
                            ValidationSummary = validationApiException.ValidatedFields.Select(valRes =>
                                        new ValidationError()
                                        {
                                            ErrorCode = valRes.ErrorCode,
                                            FieldName = valRes.MemberNames.FirstOrDefault(),
                                            Message = valRes.ErrorMessage
                                        }
                                    )
                        };

                        break;
                    }

                case ApiException apiException:
                    {
                        content = new ErrorDetails()
                        {
                            StatusCode = StatusCodes.Status500InternalServerError,
                            Message = apiException.Message
                        };
                        break;
                    }

                default:
                    {
                        content = new ErrorDetails()
                        {
                            StatusCode = StatusCodes.Status500InternalServerError,
                            Message = _env.IsDevelopment() ? exception.ToString() : "Internal Server Error",
                        };

                        break;
                    }
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = content.StatusCode;

            _logger.LogError($"Exception Message: {exception.Message}, stack trace: {exception.StackTrace}");

            return context.Response.WriteAsync(content.ToString());
        }
    }
}