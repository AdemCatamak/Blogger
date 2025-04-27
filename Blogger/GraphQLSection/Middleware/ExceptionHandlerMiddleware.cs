using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Blogger.Domain;
using Blogger.Domain.Exceptions;
using GraphQL;
using GraphQL.Instrumentation;
using Microsoft.Extensions.Logging;

namespace Blogger.GraphQLSection.Middleware
{
    public class ExceptionHandlerMiddleware : IFieldMiddleware
    {
        private readonly ILoggerFactory _loggerFactory;

        public ExceptionHandlerMiddleware(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public async Task<object?> Resolve(IResolveFieldContext context, FieldMiddlewareDelegate next)
        {
            
            object? result = default;
            try
            {
                result = await next(context);
            }
            catch (Exception e)
            {
                ILogger<ExceptionHandlerMiddleware> logger = _loggerFactory.CreateLogger<ExceptionHandlerMiddleware>();
                await HandleException(e, logger, context);
            }

            return result;
        }

        // https://github.com/graphql-dotnet/graphql-dotnet/issues/495#issuecomment-616951681
        private Task HandleException(Exception exception, ILogger logger, IResolveFieldContext context)
        {
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

            logger.Log(logLevel, exception, message);

            var metadata = new Dictionary<string, object>
                           {
                               {"typeName", context.ParentType?.Name ?? string.Empty},
                               {"source", context.FieldDefinition?.Name ?? string.Empty},
                               {"httpCode", (int)httpStatusCode},
                               {"errorCode", typeOfException ?? nameof(Exception)},
                           };
            var executionError = new ExecutionError(message, metadata)
                                 {
                                     Path = context.Path,
                                     // Code = httpStatusCode.ToString(),
                                     Source = context.Source?.ToString()
                                 };

            context.Errors.Add(executionError);

            return Task.CompletedTask;
        }
    }
}