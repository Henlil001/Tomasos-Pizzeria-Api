using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tomasos_Pizzeria.Core.Interfaces;

namespace Tomasos_Pizzeria.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Route("GettAll")]
        [HttpGet]
        public async Task<IActionResult> GetAllCategory()
        {
            return Ok(await _categoryService.GetAllCategory());
        }

        [Route("Add")]
        [HttpPost]
        public async Task<IActionResult> AddCategory(string categoryname)
        {
            bool result = await _categoryService.AddCategory(categoryname);

            return result is true ?
                Ok($"Category \"{categoryname}\" added Successfully") :
                BadRequest($"Category \"{categoryname}\" already exist");
        }
    }
}
