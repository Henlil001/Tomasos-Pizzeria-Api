using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Tomasos_Pizzeria.Core;
using Tomasos_Pizzeria.Data.Identity;

namespace Tomasos_Pizzeria.Api.Extensions
{
    public static class KeyVaultDbContextAndApplicationInsightsExtensions
    {

        public static async Task AddKeyVaultDbContextAndApplicationInsightsExtendedAsync(this WebApplicationBuilder builder)
        {
            //sätta upp keyvault
            var keyVaultURL = builder.Configuration.GetSection("KeyVault:KeyVaultURL");

            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(new AzureServiceTokenProvider().KeyVaultTokenCallback));

            builder.Configuration.AddAzureKeyVault(keyVaultURL.Value!.ToString(), new DefaultKeyVaultSecretManager());


            var client = new SecretClient(new Uri(keyVaultURL.Value!.ToString()), new DefaultAzureCredential());

            var connString = (await client.GetSecretAsync("DbConnString")).Value.Value;
            SecretKey.Key = (await client.GetSecretAsync("SeacretKeyToken")).Value.Value;
            SecretKey.AdminName = (await client.GetSecretAsync("AdminName")).Value.Value;
            SecretKey.IssuerAudience = (await client.GetSecretAsync("IssAud")).Value.Value;
            var applicationInsightsConnString = (await client.GetSecretAsync("ApplicationInsightsConnString")).Value.Value;

            //Lägger till ApplicationInsights
            builder.Services.AddApplicationInsightsTelemetry(applicationInsightsConnString);



            // Skapar upp databasen med våra entiteis-klass som blir en tabell
            builder.Services.AddDbContext<PizzeriaDbContext>(options =>
                options.UseSqlServer(connString));


           

        }
    }
}
