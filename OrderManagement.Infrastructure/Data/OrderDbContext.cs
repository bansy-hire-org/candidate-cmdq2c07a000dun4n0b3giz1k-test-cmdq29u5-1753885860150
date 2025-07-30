using Microsoft.EntityFrameworkCore;
using OrderManagement.Domain.Models;

namespace OrderManagement.Infrastructure.Services
{
    public class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure default values or relationships here
            modelBuilder.Entity<Order>().Property(o => o.OrderDate).HasDefaultValueSql("GETDATE()");
        }
    }
}