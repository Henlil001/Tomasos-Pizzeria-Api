using Tomasos_Pizzeria.Domain.DTO;
using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Data.Interfaces
{
    public interface IFoodRepo
    {
        Task AddFoodAsync(List<Food> foods);
        Task<bool> DeleteFoodAsync(int id);
        Task<List<Food>> GetAllFoodsAsync();
        Task<bool> UpdateFoodAsync(FoodDTO food);
        Task<List<Food>> GetFoodsByIdAsync(List<int> foodIds);
    }
}
