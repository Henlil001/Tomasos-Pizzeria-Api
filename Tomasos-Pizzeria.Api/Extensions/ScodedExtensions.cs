using Tomasos_Pizzeria.Core.Interfaces;
using Tomasos_Pizzeria.Core.Services;
using Tomasos_Pizzeria.Data.Interfaces;
using Tomasos_Pizzeria.Data.Repos;

namespace Tomasos_Pizzeria.Api.Extensions
{
    public static class ScodedExtensions
    {
        public static IServiceCollection AddScopedExtended(this IServiceCollection services)
        {
            services.AddScoped<IApplicationUserService, ApplicationUserService>();

            services.AddScoped<IOrderRepo, OrderRepo>();
            services.AddScoped<IFoodRepo, FoodRepo>();
            services.AddScoped<IIngredientRepo, IngredientRepo>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IFoodService, FoodService>();
            services.AddScoped<IIngredientService, IngredientService>();


            return services;
        }
    }
}
