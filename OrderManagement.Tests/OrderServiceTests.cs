using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OrderManagement.Domain.Models;
using OrderManagement.Infrastructure.Services;

namespace OrderManagement.Tests
{
    public class OrderServiceTests
    {
        private OrderDbContext _dbContext;
        private OrderService _orderService;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new OrderDbContext(options);
            _dbContext.Database.EnsureDeleted(); // Clean up the database before each test
            _dbContext.Database.EnsureCreated();

            _orderService = new OrderService(_dbContext);
        }

        [Test]
        public async Task GetAllOrdersAsync_ReturnsAllOrders()
        {
            // Arrange
            await _dbContext.Orders.AddRangeAsync(new Order { CustomerName = "Test1", ShippingAddress = "Address1", TotalAmount = 100 }, new Order { CustomerName = "Test2", ShippingAddress = "Address2", TotalAmount = 200 });
            await _dbContext.SaveChangesAsync();

            // Act
            var orders = await _orderService.GetAllOrdersAsync();

            // Assert
            Assert.AreEqual(2, orders.Count());
        }

        [Test]
        public async Task GetOrderByIdAsync_ReturnsCorrectOrder()
        {
            // Arrange
            var order1 = new Order { CustomerName = "Test1", ShippingAddress = "Address1", TotalAmount = 100 };
            await _dbContext.Orders.AddAsync(order1);
            await _dbContext.SaveChangesAsync();

            // Act
            var order = await _orderService.GetOrderByIdAsync(order1.Id);

            // Assert
            Assert.AreEqual(order1.CustomerName, order.CustomerName);
        }

        [Test]
        public async Task CreateOrderAsync_CreatesNewOrder()
        {
            // Arrange
            var newOrder = new Order { CustomerName = "Test3", ShippingAddress = "Address3", TotalAmount = 300 };

            // Act
            await _orderService.CreateOrderAsync(newOrder);

            // Assert
            var orderInDb = await _dbContext.Orders.FindAsync(newOrder.Id);
            Assert.AreEqual(newOrder.CustomerName, orderInDb.CustomerName);
        }

        [Test]
        public async Task UpdateOrderAsync_UpdatesExistingOrder()
        {
            // Arrange
            var order1 = new Order { CustomerName = "Test1", ShippingAddress = "Address1", TotalAmount = 100 };
            await _dbContext.Orders.AddAsync(order1);
            await _dbContext.SaveChangesAsync();

            order1.CustomerName = "UpdatedName";

            // Act
            await _orderService.UpdateOrderAsync(order1);

            // Assert
            var updatedOrder = await _dbContext.Orders.FindAsync(order1.Id);
            Assert.AreEqual("UpdatedName", updatedOrder.CustomerName);
        }

        [Test]
        public async Task DeleteOrderAsync_DeletesExistingOrder()
        {
            // Arrange
            var order1 = new Order { CustomerName = "Test1", ShippingAddress = "Address1", TotalAmount = 100 };
            await _dbContext.Orders.AddAsync(order1);
            await _dbContext.SaveChangesAsync();

            // Act
            await _orderService.DeleteOrderAsync(order1.Id);

            // Assert
            var deletedOrder = await _dbContext.Orders.FindAsync(order1.Id);
            Assert.IsNull(deletedOrder);
        }
    }
}