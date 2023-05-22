using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace OzonEdu.StockApi.Infrastructure.Swagger
{
    public class HeaderOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.RelativePath.ToLower().Equals("v1/api/stock") && context.ApiDescription.HttpMethod.ToLower().Equals("get"))
            {
                operation.Parameters ??= new List<OpenApiParameter>();
                operation.Parameters.Add(new OpenApiParameter
                {
                    In = ParameterLocation.Header,
                    Name = "our-header",
                    Required = false,
                    Schema = new OpenApiSchema { Type = "string" }
                });
            }
        }
    }
}