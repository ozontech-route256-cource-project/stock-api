using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace OzonEdu.StockApi.Infrastructure.ActionFilters;

public class GlobalExceptionFilter : ExceptionFilterAttribute
{
    private readonly ILogger<GlobalExceptionFilter> logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        this.logger = logger;
    }

    public override void OnException(ExceptionContext context)
    {
        base.OnException(context);
        var resultObject = new
        {
            ExceptionType = context.Exception.GetType().FullName,
            context.Exception.Message
        };
        var jsonResult = new JsonResult(resultObject)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };
        logger.LogError(context.Exception, $" {context.HttpContext.Request.Method}{context.HttpContext.Request.Path} unhandled exception ");
        context.Result = jsonResult;
    }
}