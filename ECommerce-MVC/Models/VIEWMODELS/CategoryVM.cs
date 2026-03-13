using System.ComponentModel.DataAnnotations;

namespace ECommerce_MVC.Models.VIEWMODELS
{
    public class CategoryVM
    {
        public int CategoryId {  get; set; }
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Category Name")]
        public string CategoryName { get; set; }
        [Display(Name = "Parent Category")]
        public int? ParentCategoryId {  get; set; }
        public string? ParentCategoryName { get; set; }

    }
}
