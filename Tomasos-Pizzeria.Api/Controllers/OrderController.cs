using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Tomasos_Pizzeria.Core.Interfaces;
using Tomasos_Pizzeria.Domain.DTO;

namespace Tomasos_Pizzeria.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteOrderAsync(int orderId)
        {
            bool result = await _orderService.DeleteOrderAsync(orderId);

            return result is true ?
                Ok("Order Deleted Successfully") :
                BadRequest("Order dosent exist");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("ChangeStatus")]
        public async Task<IActionResult> ChangeStatusAsync(int orderID, string newStatus)
        {
            bool result = await _orderService.UpdateOrderStatusAsync(orderID, newStatus);

            return result is true ?
                Ok($"Status changed to {newStatus}") :
                BadRequest("Order dosent exist");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAllOrdersAsync()
        {
            return Ok(await _orderService.GetAllOrdersAsync());
        }

        [Authorize]
        [HttpGet]
        [Route("GetMy")]
        public async Task<IActionResult> GetMyOrdersAsync()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (userId is null)
            {
                _logger.LogDebug("Token with no ID found");
                return StatusCode(500, "Error, Token with no ID found");
            }
            var myOrders = await _orderService.GetOrdersByUserIdAsync(userId);
            return Ok(myOrders);
        }

        [Authorize]
        [HttpPost]
        [Route("Place")]
        public async Task<IActionResult> PlaceOrderAsync(List<OrderFoodDTO> foodsWithQuantity)
        {
            if (ModelState.IsValid)
            {
                var userRole = User.FindFirst(ClaimTypes.Role).Value;
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (userRole is null || userId is null)
                {
                    _logger.LogDebug("Token with no Role/ID found");
                    return StatusCode(500, "Error, Token with no Role/IDfound");
                }
                await _orderService.PlaceOrderAsync(foodsWithQuantity, userRole, userId);

                return Ok("Order Placed Successfully");

            }
                return BadRequest();

        }
    }
}
