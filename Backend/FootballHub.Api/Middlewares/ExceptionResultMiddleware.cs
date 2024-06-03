﻿using System.Net;
using FootballHub.Api.Application.Response;
using FootballHub.Application.Exceptions;


namespace FootballHub.Api.Middlewares;

public class ExceptionResultMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionResultMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, ILogger<ExceptionResultMiddleware> logger)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ErrorException e)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            await httpContext.Response.WriteAsJsonAsync(new ErrorResponse { Error = e.Error });
        }
        catch (UnauthorizedException ue)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await httpContext.Response.WriteAsJsonAsync(new UnauthorizedResponse
                { Reason = ue.Message ?? "Unauthorized" });
        }
        catch (ValidationException ve)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            await httpContext.Response.WriteAsJsonAsync(new ValidationResponse(ve));
        }
        catch (Exception e)
        {
            logger.LogCritical(e, "Fatal error");
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await httpContext.Response.WriteAsJsonAsync("Server error");
        }
    }
}

public static class ExceptionResultMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionResultMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionResultMiddleware>();
    }   
}