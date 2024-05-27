using Microsoft.Extensions.Logging;
using Tomasos_Pizzeria.Core.Interfaces;
using Tomasos_Pizzeria.Data.Interfaces;
using Tomasos_Pizzeria.Domain.DTO;
using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Core.Services
{
    public class FoodService : IFoodService
    {
        private readonly IFoodRepo _foodRepo;
        private readonly IIngredientRepo _ingredientRepo;
        private readonly ICategoryRepo _categoryRepo;
        private readonly ILogger<FoodService> _logger;


        public FoodService(IFoodRepo foodRepo, ILogger<FoodService> logger, IIngredientRepo ingredientRepo, ICategoryRepo categoryRepo)
        {
            _foodRepo = foodRepo;
            _logger = logger;
            _ingredientRepo = ingredientRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task AddFoodsAsync(List<FoodDTO> foodsDTO)
        {
            try
            {
                var foods = new List<Food>();

                foreach (var foodDTO in foodsDTO)
                {
                    var food = new Food
                    {
                        Title = foodDTO.Title,
                        Description = foodDTO.Description,
                        Price = foodDTO.Price,
                        Category = await _categoryRepo.GetCategoryByIdAsync(foodDTO.CategoryId),
                        Ingredients = await _ingredientRepo.GetIngredientsByIdAsync(foodDTO.IngredientsId)
                    };

                    foods.Add(food);
                }

                await _foodRepo.AddFoodAsync(foods);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error occurred while trying to AddFood");
                throw;
            }
        }

        public async Task<bool> DeleteFoodAsync(int id)
        {
            try
            {
                bool result = await _foodRepo.DeleteFoodAsync(id);
                return result is true ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error occurred while trying to DeleteFood");
                throw;
            }
        }

        public async Task<List<Food>> GetAllFoodsAsync()
        {
            try
            {
                return await _foodRepo.GetAllFoodsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error occurred while trying to GetAllFoods");
                throw;
            }
        }

        public async Task<bool> UpdateFoodAsync(FoodDTO food)
        {
            try
            {
                bool result = await _foodRepo.UpdateFoodAsync(food);
                return result is true ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error occurred while trying to UpdateFoodA");
                throw;
            }
        }
    }
}
