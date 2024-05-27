using System.Text.Json.Serialization;

namespace Tomasos_Pizzeria.Api.Extensions
{
    public static class ControllersExtensions
    {
        public static IServiceCollection AddControllerExtension(this IServiceCollection services)
        {
            services.AddControllers()
                        .AddJsonOptions(options =>
                        {
                            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
                            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                            options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                        });
            return services;
        }
    }
}
