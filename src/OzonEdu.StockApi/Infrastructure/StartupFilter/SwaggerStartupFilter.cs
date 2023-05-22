
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using System;

namespace OzonEdu.StockApi.Infrastructure.StartupFilter;

public class SwaggerStartupFilter : IStartupFilter
{
    public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
    {
        return app =>
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            next(app);
        };
    }
}