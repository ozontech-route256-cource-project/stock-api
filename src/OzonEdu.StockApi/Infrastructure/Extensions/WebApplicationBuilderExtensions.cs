
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using OzonEdu.StockApi.Infrastructure.ActionFilters;
using OzonEdu.StockApi.Infrastructure.StartupFilter;
using OzonEdu.StockApi.Infrastructure.Swagger;
using System;
using System.IO;
using System.Reflection;

namespace OzonEdu.StockApi.Infrastructure.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddInfrastrusture(this WebApplicationBuilder builder)
    {
        var services = builder.Services;
        services.AddSingleton<IStartupFilter, SwaggerStartupFilter>();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "OzonEdu.StockApi", Version = "v1", Description = "Описание" });
            //options.CustomSchemaIds(x => x.FullName);
            var xmlFileName = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
            var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
            options.IncludeXmlComments(xmlFilePath);
            options.OperationFilter<HeaderOperationFilter>();
        });
        return builder;
    }

    public static WebApplicationBuilder AddControllersWithExceptionFilter(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
        return builder;
    }

}