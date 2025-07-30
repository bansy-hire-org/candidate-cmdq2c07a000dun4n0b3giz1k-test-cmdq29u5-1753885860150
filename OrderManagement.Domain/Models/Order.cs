using System.ComponentModel.DataAnnotations;

namespace OrderManagement.Domain.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string ShippingAddress { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }
    }
}