using Microsoft.AspNetCore.Identity;
using Tomasos_Pizzeria.Api.Extensions;
using Tomasos_Pizzeria.Data.Identity;

namespace Tomasos_Pizzeria.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Extension Method from static Class ControllersExtensions
            builder.Services.AddControllerExtension();

            //Extension Method from static Class KeyVaultAndDbContextExtensions
            await builder.AddKeyVaultDbContextAndApplicationInsightsExtendedAsync();

            //S�tter upp Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
               .AddEntityFrameworkStores<PizzeriaDbContext>()
               .AddDefaultTokenProviders();

            //Extension Method from static Class JwtAuthentication
            builder.Services.AddJwtAuthenticationExtended(builder.Configuration);

            //Extension Method from static Class ScopedExtension
            builder.Services.AddScopedExtended();

            //Extension Method from static Class SwaggerExtensions
            builder.Services.AddSwaggerExtended();

            var app = builder.Build();

            // Detta m�ste g�ras innan routing
            app.UseAuthentication();

            // Extension Method from static Class SwaggerExtensions
            app.UseSwaggerExtended();

            // Anv�nd Routing
            app.UseRouting();

            // Anv�nd Authorization
            app.UseAuthorization();

            // F�r att anv�nda https leder om fr�n port 80 till 443
            app.UseHttpsRedirection();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            //L�gger till de roller som ska finnas om dom inte redan finns
            await app.InitializeRolesAsync();

            await app.RunAsync();
        }
    }
}
