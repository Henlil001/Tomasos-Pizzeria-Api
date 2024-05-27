using Microsoft.OpenApi.Models;

namespace Tomasos_Pizzeria.Api.Extensions
{
    public static class SwaggerExtensions
    {
        //Builder.Service där har vi IServiceCollection
        public static IServiceCollection AddSwaggerExtended(this IServiceCollection services)
        {
            //Här läggs all konfigurering av services  
            services.AddSwaggerGen(options =>
            {
                //Generell konfigurering av Swagger
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "My Swagger API",
                        Version = "v1"
                    }
                );

                //Konfigurering som har att göra med att använda JWT tokens direkt i gränssnittet
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "JWT token"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            { Type = ReferenceType.SecurityScheme,
                              Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            });

            return services;
        }
        //Pipline (app) där har vi AApplicationBuilder
        public static IApplicationBuilder UseSwaggerExtended(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "My Swagger API");
                options.RoutePrefix = string.Empty;
            });

            return app;
        }       
    }
}
