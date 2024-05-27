using Microsoft.EntityFrameworkCore;
using Tomasos_Pizzeria.Data.Identity;
using Tomasos_Pizzeria.Data.Interfaces;
using Tomasos_Pizzeria.Domain.Entities;

namespace Tomasos_Pizzeria.Data.Repos
{
    public class OrderRepo : IOrderRepo
    {
        private readonly PizzeriaDbContext _context;

        public OrderRepo(PizzeriaDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            var selectedOrder = await _context.Orders
                                .Include(o => o.Foods)
                                .SingleOrDefaultAsync(o => o.OrderID == orderId);

            if (selectedOrder is null)
                return false;

            // Ta bort ordern från alla maträtter som den har
            foreach (var food in selectedOrder.Foods)
            {
                food.Orders.Remove(selectedOrder);
            }

            _context.Orders.Remove(selectedOrder);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.Orders
                    .Include(o => o.Foods)
                    .ThenInclude(f => f.Category)
                    .AsNoTracking()
                    .ToListAsync();
        }

        public async Task<List<Order>> GetOrderByUserIdAsync(string userId)
        {
            return await _context.Orders
                       .Include(o => o.Foods)
                           .ThenInclude(f => f.Category)  // Inkludera den virtuella egenskapen i Foods
                       .AsNoTracking()
                       .Where(o => o.ApplicationUserId == userId)
                       .ToListAsync();
        }

        public async Task PlaceOrderAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId, string Status)
        {
            var selectedOrder = await _context.Orders.SingleOrDefaultAsync(o => o.OrderID == orderId);
            if (selectedOrder is null)
                return false;

            selectedOrder.Status = Status;
            await _context.SaveChangesAsync();

            return true;

        }
    }
}
