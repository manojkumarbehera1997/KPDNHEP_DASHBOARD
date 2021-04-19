using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KPDNHEP.Console.Services.Models
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class MiddlewareAuthorization
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public MiddlewareAuthorization(RequestDelegate next, ILoggerFactory logFactory)
        {
            _next = next;
            _logger = logFactory.CreateLogger("MyMiddleware");
        }

        public async Task Invoke(HttpContext httpContext)
        {
            _logger.LogInformation("MyMiddleware executing..");
            await _next(httpContext);// calling next middleware
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareAuthorizationExtensions
    {
        public static IApplicationBuilder UseMiddlewareAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MiddlewareAuthorization>();
        }
    }
}
