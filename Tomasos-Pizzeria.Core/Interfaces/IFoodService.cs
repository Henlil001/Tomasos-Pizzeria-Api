using Tomasos_Pizzeria.Domain.DTO;
using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Core.Interfaces
{
    public interface IFoodService
    {
        Task AddFoodsAsync(List<FoodDTO> foods);
        Task<bool> DeleteFoodAsync(int id);
        Task<List<Food>> GetAllFoodsAsync();
        Task<bool> UpdateFoodAsync(FoodDTO food);
    }
}
