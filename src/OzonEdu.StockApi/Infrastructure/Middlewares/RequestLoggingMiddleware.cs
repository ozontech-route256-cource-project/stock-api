using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Text;
using System.Threading.Tasks;

namespace OzonEdu.StockApi.Infrastructure.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<RequestLoggingMiddleware> logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            await LogRequest(context);
            await next(context);
        }

        private async Task LogRequest(HttpContext context)
        {
            try
            {
                if(context.Request.ContentLength > 0)
                {
                    context.Request.EnableBuffering();
                    var buffer = new byte[context.Request.ContentLength.Value];
                    await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
                    var bodyAsText = Encoding.UTF8.GetString(buffer);
                    logger.LogInformation("Request logged");
                    logger.LogInformation(bodyAsText);
                    context.Request.Body.Position = 0;
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Could not log request body");
            }
        }
    }
}
