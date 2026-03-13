using System.ComponentModel.DataAnnotations;

namespace ECommerce_MVC.Models.Model
{
    public class OrderItem
    {
        [Key]
        public int OrderItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }

        // Foreign Keys
        public int OrderId { get; set; }
        public int ProductId { get; set; }

        // Navigation Properties
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
       
    }
}