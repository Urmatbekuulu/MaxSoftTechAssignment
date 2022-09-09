using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace MaxSoftTechAssignment.WEB.Configurations;

public static class SwaggerConfiguration
{
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(setup=>
        {
    
            setup.MapType(typeof(TimeSpan),()=>new OpenApiSchema()
            {
                Type = "string",
                Example = new OpenApiString("00:00:01")
            });
            setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
            });
    
            var scheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer",
                },
            };
            setup.EnableAnnotations();
            
            setup.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "yk.61.registration",
                Version = "v1",
            });
            setup.CustomSchemaIds(x => x.FullName);
            setup.DocInclusionPredicate((_, _) => true);
            
           
    
        });
        services.AddSwaggerGenNewtonsoftSupport();

    }
}