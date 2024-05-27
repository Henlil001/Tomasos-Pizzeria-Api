using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Data.Interfaces
{
    public interface ICategoryRepo
    {
        Task<List<Category>> GetCategoriesByIdsAsync(List<int> ids);
        Task<Category> GetCategoryByIdAsync(int id);
        Task<bool> AddCategory(Category category);
        Task<List<Category>> GetAllCategories();
    }
}
