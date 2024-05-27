using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Data.Interfaces
{
    public interface IIngredientRepo
    {
        Task<bool> AddIngredientAsync(string IngridentName);
        Task<bool> UppdateIngredientAsync(int ingredientId, string newIngrident);
        Task<List<Ingredient>> GetAllIngredientsAsync();
        Task<List<Ingredient>> GetIngredientsByIdAsync(List<int> ingreIds);
    }
}
