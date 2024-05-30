using Microsoft.EntityFrameworkCore;
using Tomasos_Pizzeria.Data.Identity;
using Tomasos_Pizzeria.Data.Interfaces;
using Tomasos_Pizzeria.Domain.DTO;
using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Data.Repos
{
    public class FoodRepo : IFoodRepo
    {
        private readonly PizzeriaDbContext _context;

        public FoodRepo(PizzeriaDbContext context)
        {
            _context = context;
        }

        public async Task AddFoodAsync(List<Food> foods)
        {
            foreach (var food in foods)
            {
                _context.Foods.Add(food);
            }
            await _context.SaveChangesAsync();
        }

        public async Task<bool> DeleteFoodAsync(int id)
        {
            var foodToDelete = await _context.Foods
             .Include(f => f.FoodOrders) // Include the junction table
             .ThenInclude(fo => fo.Order)
             .Include(f => f.Ingredients)
             .Include(f => f.Category)
             .SingleOrDefaultAsync(f => f.FoodID == id);

            if (foodToDelete == null)
                return false;

            // Remove the food from its category
            if (foodToDelete.Category != null)
            {
                var category = await _context.Categories
                                    .Include(c => c.Foods)
                                    .SingleOrDefaultAsync(c => c.CategoryID == foodToDelete.Category.CategoryID);
                if (category != null)
                {
                    if (category.Foods != null)
                    {
                        category.Foods.Remove(foodToDelete);
                    }
                }
            }

            // Remove the food from all orders
            foreach (var foodOrder in foodToDelete.FoodOrders)
            {
                var order = await _context.Orders
                    .Include(o => o.FoodOrders)
                    .SingleOrDefaultAsync(o => o.OrderID == foodOrder.OrderID);
                if (order != null)
                {
                    order.FoodOrders.Remove(foodOrder);
                }
            }

            // Remove the food from all ingredients
            foreach (var ingredient in foodToDelete.Ingredients)
            {
                if(ingredient.Foods != null)
                ingredient.Foods.Remove(foodToDelete);
            }

            // Remove all FoodOrder entries related to this food
            _context.FoodOrders.RemoveRange(foodToDelete.FoodOrders);

            // Finally, remove the food itself
            _context.Foods.Remove(foodToDelete);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Food>> GetAllFoodsAsync()
        {
            return await _context.Foods
                        .Include(f => f.Ingredients)
                        .Include(f => f.Category)
                        .OrderBy(f => f.Category.CategoryID)
                        .AsNoTracking()
                        .ToListAsync();
        }
        public async Task<bool> UpdateFoodAsync(FoodDTO foodDTO)
        {
            //Här kan maträtt uppdateras (namn, pris categori, ingredienser, beskrivning)
            //Men inte vilka ordrar den tillhör

            //Hämtar maträtten som vi vill uppdatera medföljande ordrar 
            var selectedFood = await _context.Foods
                        .Include(f => f.FoodOrders)
                        .ThenInclude(fo=>fo.Order)
                        .SingleOrDefaultAsync(f => f.FoodID == foodDTO.FoodID);
            //Hämtar den nya kategorin
            var category = await _context.Categories
                .SingleOrDefaultAsync(c => c.CategoryID.Equals(foodDTO.CategoryId));
            //Hämtar de nya Ingridienseran
            var ingredients = await _context.Ingredients
                                .Where(i => foodDTO.IngredientsId.Contains(i.IngredientID))
                                .ToListAsync();

            if (selectedFood is null || category is null || ingredients is null)
                return false;

            var food = new Food
            {
                FoodID = selectedFood.FoodID,
                Title = foodDTO.Title,
                Description = foodDTO.Description,
                Price = foodDTO.Price,
                Orders = selectedFood.Orders,
                Ingredients = ingredients,
                Category = category,
            };

            _context.Entry(selectedFood).CurrentValues.SetValues(food);
            await _context.SaveChangesAsync();

            return true;

        }
        public async Task<List<Food>> GetFoodsByIdAsync(List<int> foodIds)
        {
            var foods = await _context.Foods //om det är samma id två gånger tar den bara med det 1 gång måste fixa de
                                 .Where(f => foodIds.Contains(f.FoodID))
                                 .Include(f => f.Category)
                                 .Include(f => f.Ingredients)
                                 .ToListAsync();
            // Create a dictionary to store the fetched foods by their ID
            var foodsById = foods.ToDictionary(f => f.FoodID);

            // Create the result list maintaining the order and frequency of the input IDs
            var result = new List<Food>();
            foreach (var id in foodIds)
            {
                if (foodsById.TryGetValue(id, out var food))
                {
                    result.Add(food);
                }
            }

            return result;


        }
    }
}
