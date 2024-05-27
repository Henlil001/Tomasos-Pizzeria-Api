using Microsoft.EntityFrameworkCore;
using Tomasos_Pizzeria.Data.Identity;
using Tomasos_Pizzeria.Data.Interfaces;
using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Data.Repos
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly PizzeriaDbContext _context;

        public CategoryRepo(PizzeriaDbContext context)
        {
            _context = context;
        }

        public Task<List<Category>> GetCategoriesByIdsAsync(List<int> ids)
        {
            throw new NotImplementedException();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            var category = await _context.Categories.SingleOrDefaultAsync(c => c.CategoryID.Equals(id));

            return category;
        }
        public async Task<bool> AddCategory(Category category)
        {
            var selectedCategory = await _context.Categories.SingleOrDefaultAsync(c => c.Name.Equals(category.Name));
            if (selectedCategory != null)
                return false;

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<List<Category>> GetAllCategories()
        {
            return await _context.Categories.AsNoTracking().ToListAsync();
        }
    }
}
