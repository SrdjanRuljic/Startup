using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebAPI.Helpers
{
    public class SwaggerLanguageHeader : IOperationFilter
    {
        private readonly IServiceProvider _serviceProvider;

        public SwaggerLanguageHeader(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters?.Add(new OpenApiParameter
            {
                Name = "Accept-Language",
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema
                {
                    Type = "string",
                    Enum = new List<IOpenApiAny>
                    {
                        new OpenApiString("en"),
                        new OpenApiString("sr"),
                        new OpenApiString("ba")
                    }
                }
            });
        }
    }
}