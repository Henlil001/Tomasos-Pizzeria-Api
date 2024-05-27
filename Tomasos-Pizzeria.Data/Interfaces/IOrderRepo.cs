using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Data.Interfaces
{
    public interface IOrderRepo
    {
        Task PlaceOrderAsync(Order order);
        Task<bool> UpdateOrderStatusAsync(int orderId, string Status);
        Task<List<Order>> GetAllOrdersAsync();
        Task<List<Order>> GetOrderByUserIdAsync(string userId);
        Task<bool> DeleteOrderAsync(int orderId);
    }
}
