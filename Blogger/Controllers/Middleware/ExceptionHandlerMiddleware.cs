using System;
using System.Net;
using System.Threading.Tasks;
using Blogger.Domain;
using Blogger.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Blogger.Controllers.Middleware
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ExceptionHandlerMiddleware> logger)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception e)
            {
                await HandleException(e, logger, context);
            }
        }

        private async Task HandleException(Exception exception, ILogger logger, HttpContext context)
        {
            string traceIdentifier = context.TraceIdentifier;
            string message = exception.Message;
            string? typeOfException = exception.GetType().GetFriendlyName();
            HttpStatusCode httpStatusCode;
            LogLevel logLevel;

            if (exception is ValidationException)
            {
                httpStatusCode = HttpStatusCode.BadRequest;
                logLevel = LogLevel.Information;
            }
            else if (exception is NotFoundException)
            {
                httpStatusCode = HttpStatusCode.NotFound;
                logLevel = LogLevel.Information;
            }
            else
            {
                message = "Unknown error occurs";
                typeOfException = null;
                httpStatusCode = HttpStatusCode.InternalServerError;
                logLevel = LogLevel.Error;
            }

            logger.Log(logLevel,  exception,$"TraceId: {traceIdentifier}{Environment.NewLine}{exception.Message}");

            var errorHttpResponse = new ErrorHttpResponse(message, typeOfException);
            string errorHttpContentStr = JsonConvert.SerializeObject(errorHttpResponse);

            context.Response.StatusCode = (int) httpStatusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(errorHttpContentStr);
        }

        public class ErrorHttpResponse
        {
            public ErrorHttpResponse(string friendlyMessage, string? exceptionType = null)
            {
                FriendlyMessage = friendlyMessage ?? throw new ArgumentNullException(nameof(friendlyMessage));
                ExceptionType = exceptionType;
            }

            public string FriendlyMessage { get; }
            public string? ExceptionType { get; }
        }
    }
}