using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tomasos_Pizzeria.Core.Interfaces;
using Tomasos_Pizzeria.Domain.DTO;

namespace Tomasos_Pizzeria.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;

        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> AddFoodAsync(List<FoodDTO> foods)
        {
            await _foodService.AddFoodsAsync(foods);
            return Ok("Food Added");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("Uppdate")]
        public async Task<IActionResult> UppdateFoodAsync(FoodDTO food)
        {
            bool result = await _foodService.UpdateFoodAsync(food);

            return result is true ?
                Ok("Uppdated Successfully") :
                BadRequest("Food Dosent exist");
        }

        [Authorize]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllFoodsAsync()
        {
            return Ok(await _foodService.GetAllFoodsAsync());
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteFoodAsync(int id)
        {
            bool result = await _foodService.DeleteFoodAsync(id);

            return result is true ?
                Ok("Food was deleted") :
                BadRequest("Food dosent exist, BadRequest");
        }
    }
}
