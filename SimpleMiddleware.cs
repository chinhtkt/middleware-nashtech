
using System;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading;

public class SimpleMiddleware
{

    // Name of the Response Header, Custom Headers starts with "X-"  
    private const string RESPONSE_HEADER_RESPONSE_TIME = "X-Response-Time-ms";
    // Handle to the next Middleware in the pipeline  
    private readonly RequestDelegate _next;
    public SimpleMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    public Task InvokeAsync(HttpContext context)
    {
        // Start the Timer using Stopwatch  
        var watch = new Stopwatch();
        watch.Start();
        context.Response.OnStarting(() =>
        {
            // Stop the timer information and calculate the time   
            watch.Stop();
            var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;
            // Add the Response time information in the Response headers.   
            context.Response.Headers[RESPONSE_HEADER_RESPONSE_TIME] = responseTimeForCompleteRequest.ToString();
            return Task.CompletedTask;
        });
        // Call the next delegate/middleware in the pipeline   
        return this._next(context);
    }

}

public static class MyMiddlewareExtensions
{
    public static IApplicationBuilder UseSimpleMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<SimpleMiddleware>();
    }
}



    

