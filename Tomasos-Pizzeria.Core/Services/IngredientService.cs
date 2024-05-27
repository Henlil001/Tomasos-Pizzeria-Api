using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Tomasos_Pizzeria.Core.Interfaces;
using Tomasos_Pizzeria.Data.Interfaces;
using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Core.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepo _repo;
        private readonly ILogger<IngredientService> _logger;

        public IngredientService(IIngredientRepo repo, ILogger<IngredientService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<bool> AddIngredient(string IngridentName)
        {
            try
            {
                bool result = await _repo.AddIngredientAsync(IngridentName);
                return result is true ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error occurred while trying to Add Ingredient");
                throw;
            }
        }

        public async Task<List<Ingredient>> GetAllIngredientsAsync()
        {
            try
            {
                return await _repo.GetAllIngredientsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error occurred while trying to Get All Ingredients");
                throw;
            }
        }

        public async Task<bool> UpdateIngredientAsync(int ingredientId, string newIngrident)
        {
            try
            {
                if (newIngrident.IsNullOrEmpty())
                    return false;

                bool result = await _repo.UppdateIngredientAsync(ingredientId, newIngrident);
                return result is true ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error occurred while trying to Update Ingredient");
                throw;
            }
        }
    }
}
