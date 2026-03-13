using System.ComponentModel.DataAnnotations;

namespace ECommerce_MVC.Models.Model
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public string Name { get; set; }

        // Self-referencing relationship
        public int? ParentCategoryId { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; }

        // Navigation to Products
        public virtual ICollection<Product> Products { get; set; }
    }
}