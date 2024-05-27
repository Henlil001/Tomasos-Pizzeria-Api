using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Core.Interfaces
{
    public interface IIngredientService
    {
        Task<bool> AddIngredient(string IngridentName);
        Task<List<Ingredient>> GetAllIngredientsAsync();
        Task<bool> UpdateIngredientAsync(int ingredientId, string newIngrident);
    }
}
