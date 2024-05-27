using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Core.Interfaces
{
    public interface ICategoryService
    {
        Task<List<Category>> GetAllCategory();
        Task<bool> AddCategory(string newCategory);
    }
}
