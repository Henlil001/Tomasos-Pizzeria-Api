using Microsoft.EntityFrameworkCore;
using Tomasos_Pizzeria.Data.Identity;
using Tomasos_Pizzeria.Data.Interfaces;
using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Data.Repos
{
    public class IngredientRepo : IIngredientRepo
    {
        private readonly PizzeriaDbContext _context;

        public IngredientRepo(PizzeriaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddIngredientAsync(string ingridentName)
        {
            var selectedIngredient = await _context.Ingredients.SingleOrDefaultAsync(i => i.Name.ToLower() == ingridentName.ToLower());
            if (selectedIngredient != null)
                return false;

            var ingredient = new Ingredient { Name = ingridentName };
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Ingredient>> GetAllIngredientsAsync()
        {
            return await _context.Ingredients.Include(i=>i.Foods).AsNoTracking().ToListAsync();
        }

        public async Task<bool> UppdateIngredientAsync(int ingredientId, string newIngrident)
        {
            var selectedIngredient = await _context.Ingredients
                        .FirstOrDefaultAsync(i => i.IngredientID == ingredientId);

            if (selectedIngredient == null)
                return false;

            // Uppdatera namnet på ingrediensen direkt
            selectedIngredient.Name = newIngrident;

            // Markera entiteten som modifierad
            _context.Entry(selectedIngredient).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Ingredient>> GetIngredientsByIdAsync(List<int> ingreIds)
        {
            var ingredients = await _context.Ingredients
                                            .Where(i => ingreIds
                                            .Contains(i.IngredientID))
                                            .ToListAsync();

            return ingredients;
        }
    }
}
