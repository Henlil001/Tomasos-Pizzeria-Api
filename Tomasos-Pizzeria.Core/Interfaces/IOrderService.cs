﻿using Tomasos_Pizzeria.Domain.DTO;
using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Core.Interfaces
{
    public interface IOrderService
    {
        Task<bool> DeleteOrderAsync(int orderId);
        Task<List<Order>> GetAllOrdersAsync();
        Task<List<Order>> GetOrdersByUserIdAsync(string userId);
        Task PlaceOrderAsync(List<OrderFoodDTO> foods, string userRole, string userId);
        Task<bool> UpdateOrderStatusAsync(int orderId, string Status);
    }
}
