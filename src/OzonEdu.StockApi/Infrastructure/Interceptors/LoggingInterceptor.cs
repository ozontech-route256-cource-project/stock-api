using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace OzonEdu.StockApi.Infrastructure.Interceptors;

public class LoggingInterceptor : Interceptor
{
    private readonly ILogger<LoggingInterceptor> logger;

    public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
    {
        this.logger = logger;
    }

    public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)

    {
        try
        {
            logger.LogInformation(JsonSerializer.Serialize(request));
        }
        catch (Exception ex)
        {
            logger.LogError($"{ex.Message}");
        }

        var response = base.UnaryServerHandler(request, context, continuation);
        try
        {
            logger.LogInformation(JsonSerializer.Serialize(response));
        }
        catch (Exception ex)
        {
            logger.LogError($"{ex.Message}");
        }

        return response;
    }
}