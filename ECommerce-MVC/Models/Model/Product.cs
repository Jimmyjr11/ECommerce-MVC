using System.ComponentModel.DataAnnotations;

namespace ECommerce_MVC.Models.Model
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string ImageUrl { get; set; }

        // Foreign Key
        public int CategoryId { get; set; }
        // Navigation Property
        public virtual Category Category { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}