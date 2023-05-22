using Microsoft.AspNetCore.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace OzonEdu.StockApi.Infrastructure.Middlewares
{
    public class VersionMiddleware
    {
        private readonly RequestDelegate next;

        public VersionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version.ToString() ?? "no version";
            await context.Response.WriteAsync(version);
        }
    }
}
