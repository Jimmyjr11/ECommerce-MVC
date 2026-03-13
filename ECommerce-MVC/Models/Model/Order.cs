using System.ComponentModel.DataAnnotations;

namespace ECommerce_MVC.Models.Model
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public int Status { get; set; } // Can be converted to Enum later
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        // Foreign Keys
        public string UserId { get; set; }
        public int ShippingAddressId { get; set; }

        // Navigation Properties
        public virtual AppUser User { get; set; }
        public virtual Address ShippingAddress { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}