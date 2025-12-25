namespace Reward_Flow_v2.Common.ErrorHandling;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Reward_Flow_v2.Common.BusinessRuleEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

internal class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private const string? SERVER_ERROR = "Server Error";
    private const string ERROR_OCCURED_MESSAGE = "An error occurred.";

    private static readonly Action<ILogger, string, Exception> LogException =
        LoggerMessage.Define<string>(LogLevel.Error, eventId: new EventId(0, "Error"), formatString: "{Message}");

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        LogException(logger, ERROR_OCCURED_MESSAGE, exception);

        var problemDetails = exception switch
        {
            BusinessRuleValidationException businessRuleValidationException => new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = businessRuleValidationException.Message
            },

            _ => new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = SERVER_ERROR

            },
        };

        httpContext.Response.StatusCode = problemDetails.Status!.Value;
        await httpContext.Response.WriteAsJsonAsync(problemDetails);

        return true;
    }
}
