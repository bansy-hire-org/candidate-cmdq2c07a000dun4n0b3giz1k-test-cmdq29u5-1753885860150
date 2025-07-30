using OrderManagement.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace OrderManagement.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly OrderDbContext _dbContext;

        public OrderService(OrderDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            return await _dbContext.Orders.FindAsync(id);
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
            return order;
        }

        public async Task UpdateOrderAsync(Order order)
        {
            _dbContext.Entry(order).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteOrderAsync(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);
            if (order != null)
            {
                _dbContext.Orders.Remove(order);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Order not found");
            }
        }
    }
}