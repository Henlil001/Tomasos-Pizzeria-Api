using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tomasos_Pizzeria.Core.Interfaces;

namespace Tomasos_Pizzeria.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientController : ControllerBase
    {
        private readonly IIngredientService _ingredientService;

        public IngredientController(IIngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddIngredientAsync(string ingridentName)
        {
            bool result = await _ingredientService.AddIngredient(ingridentName);

            return result is true ?
                Ok($"Ingredient \"{ingridentName}\" added Successfully") :
                BadRequest($"Ingredient \"{ingridentName}\" already exist");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("Uppdate")]
        public async Task<IActionResult> UppdateIngredientAsync(int ingredientId, string newIngrident)
        {
            bool result = await _ingredientService.UpdateIngredientAsync(ingredientId, newIngrident);

            return result is true ?
                Ok($"Ingredient updated to \"{newIngrident}\"") :
                BadRequest("Ingredient dosent exist/ BadRequest");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllIngredientsAsync()
        {
            return Ok(await _ingredientService.GetAllIngredientsAsync());
        }
    }
}
