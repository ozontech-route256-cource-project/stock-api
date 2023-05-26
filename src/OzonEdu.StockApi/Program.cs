
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using OzonEdu.StockApi.GrpcServices;
using OzonEdu.StockApi.Infrastructure.Extensions;
using OzonEdu.StockApi.Infrastructure.Interceptors;
using OzonEdu.StockApi.Infrastructure.Middlewares;
using OzonEdu.StockApi.Services;

namespace OzonEdu.StockApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var services = builder.Services;

        builder.AddInfrastrusture();
        builder.AddControllersWithExceptionFilter();

        services.AddSingleton<IStockService, StockService>();
        services.AddGrpc(options => options.Interceptors.Add<LoggingInterceptor>());

        var app = builder.Build();
        app.UseMiddleware<RequestLoggingMiddleware>();
        app.Map("/version", builder => builder.UseMiddleware<VersionMiddleware>());
        app.UseHttpsRedirection();
        app.UseRouting();
        app.MapControllers();
        app.MapGrpcService<StockApiGrpcService>();
        app.Run();
    }
}
