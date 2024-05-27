using Microsoft.Extensions.Logging;
using Tomasos_Pizzeria.Core.Interfaces;
using Tomasos_Pizzeria.Data.Interfaces;
using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Core.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepo _categoryRepo;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepo categoryRepo, ILogger<CategoryService> logger)
        {
            _categoryRepo = categoryRepo;
            _logger = logger;
        }

        public async Task<bool> AddCategory(string newCategory)
        {
            try
            {
                var category = new Category { Name = newCategory };
                bool result = await _categoryRepo.AddCategory(category);
                return result is true ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error occurred when procesing \"AddCategory\"");
                throw;
            }
        }

        public async Task<List<Category>> GetAllCategory()
        {
            try
            {
                return await _categoryRepo.GetAllCategories();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error occurred while trying to GetAllCategories");
                throw;
            }
        }
    }
}
