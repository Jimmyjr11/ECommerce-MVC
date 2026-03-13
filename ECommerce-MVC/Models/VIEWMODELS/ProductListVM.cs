using ECommerce_MVC.Models.Model;
using X.PagedList;
using System.Collections.Generic;

namespace ECommerce_MVC.Models.VIEWMODELS
{
    public class ProductListVM
    {
        public IPagedList<Product> Products { get; set; }

        public IEnumerable<Category> Categories { get; set; }
        public int? CurrentCategoryId { get; set; }
        public string CurrentSearchQuery { get; set; }
    }
}