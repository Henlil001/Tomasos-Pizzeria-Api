using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Tomasos_Pizzeria.Core.Interfaces;
using Tomasos_Pizzeria.Data.Identity;
using Tomasos_Pizzeria.Data.Interfaces;
using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Core.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepo _orderRepo;
        private readonly IFoodRepo _foodRepo;
        private readonly ILogger<OrderService> _logger;
        private UserManager<ApplicationUser> _userManager;

        public OrderService(IOrderRepo orderRepo, ILogger<OrderService> logger, UserManager<ApplicationUser> userManager, IFoodRepo foodRepo)
        {
            _orderRepo = orderRepo;
            _logger = logger;
            _userManager = userManager;
            _foodRepo = foodRepo;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            try
            {
                bool result = await _orderRepo.DeleteOrderAsync(orderId);
                return result is true ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error occurred while trying to DeleteOrder");
                throw;
            }
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            try
            {
                return await _orderRepo.GetAllOrdersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error occurred while trying to GetAllOrders");
                throw;
            }
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(string userId)
        {
            try
            {
                return await _orderRepo.GetOrderByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error occurred while trying to GetOrdersByUserId");
                throw;
            }
        }

        public async Task PlaceOrderAsync(List<int> foodsId, string userRole, string userId)
        {
            _logger.LogError("Någon lägger en order"); //För att se att jag kan logga till Application Insights
            try
            {
                var foods = await _foodRepo.GetFoodsByIdAsync(foodsId);
                var order = new Order()
                {
                    ApplicationUserId = userId,
                    Foods = foods,
                    Status = "Order Registered",
                    Date = DateTime.Now,
                    TotalPrice = foods.Sum(f => f.Price),
                    Discount = false,
                };
                decimal totalPrice = order.TotalPrice;

                if (userRole == "PremiumUser")
                {
                    int restPoint = 0;
                    int freeItemsGiven = 0;
                    var user = await _userManager.FindByIdAsync(userId);
                    int foodCount = foods.Count;

                    //Sorterar maträtter efter pris för att lätt kommat åt den billigaste maträtten
                    var sortedFoods = foods.OrderBy(f => f.Price).ToList();

                    // Uppdaterar UserPoints
                    for (int i = 0; i < foodCount; i++)
                    {
                        if (user.Points < 100)
                            user.Points += 10;
                        else
                            restPoint += 10;
                    }

                    bool hasBulkDiscount = foodCount >= 3;
                    bool hasFreeItem = user.Points >= 100;

                    while (hasFreeItem || restPoint >= 100)
                    {
                        order.TotalPrice -= sortedFoods[freeItemsGiven].Price;
                        freeItemsGiven++;
                        user.Points = 0;

                        if (restPoint >= 100)
                        {
                            restPoint -= 100;
                        }
                        else
                        {
                            order.Discount = true;
                            hasFreeItem = false;
                        }
                    }

                    if (hasBulkDiscount)  //Lägger till 20 % rabbat om man köpt 3 eller fler maträtter
                    {
                        order.TotalPrice = order.TotalPrice * 0.8m;
                        order.Discount = true;
                    }
                    //Räknar ut de totala rabaterade priset.
                    order.DiscountAmount = totalPrice - order.TotalPrice;
                    // Lägg till de resterande poängen om sådanna finns
                    user.Points += restPoint;
                    await _userManager.UpdateAsync(user);
                }
                //Lägger till ordern för både Regular, Premium och Admin
                await _orderRepo.PlaceOrderAsync(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error occurred while trying to PlaceOrder");
                throw;
            }
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string Status)
        {
            try
            {
                var result = await _orderRepo.UpdateOrderStatusAsync(orderId, Status);
                return result is true ? true : false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An Error occurred while trying to UpdateOrderStatus");
                throw;
            }
        }
    }


}

