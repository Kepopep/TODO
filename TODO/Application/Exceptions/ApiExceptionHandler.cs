using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TODO.Application.Exceptions;

public class ApiExceptionHandler : IExceptionHandler
{
    private readonly IProblemDetailsService _problemDetail;

    public ApiExceptionHandler(IProblemDetailsService problemDetail)
    {
        _problemDetail = problemDetail;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var errorStatusCode = GetStatusCode(exception);
        httpContext.Response.StatusCode = errorStatusCode;

        var details = new ProblemDetails
        {
            Status = errorStatusCode,
        };
        details.Extensions["traceid"] = httpContext.TraceIdentifier;
        details.Extensions["timestamp"] = DateTime.UtcNow;

        return await _problemDetail.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            ProblemDetails = details
        });
    }

    private int GetStatusCode(Exception exception)
    {
        return exception switch
        {
            DomainException => StatusCodes.Status400BadRequest,
            NotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status404NotFound,
            Exception => StatusCodes.Status500InternalServerError
        };
    }
}