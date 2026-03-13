using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ECommerce_MVC.Models.VIEWMODELS
{
    public class ProductVM
    {
        public int ProductId { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [Required]
        public string SKU { get; set; }

        [Range(0.01, 100000)]
        public decimal Price { get; set; }

        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        public bool IsActive { get; set; } = true;

        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public string? CategoryName { get; set; }
    }
}
