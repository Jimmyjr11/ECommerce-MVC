using ECommerce_MVC.Models.Model;
using ECommerce_MVC.Models.Repos;
using ECommerce_MVC.Models.VIEWMODELS;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;
using X.PagedList.Extensions;

namespace ECommerce_MVC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IUnitOfWork uow;

        public CatalogController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IActionResult> Index(int? categoryId,string q , int? page)
        {
            var products=await uow.products.GetAllAsync();
            var categories=await uow.Categories.GetAllAsync();
            if(categoryId.HasValue)
            {
                products=products.Where(p=>p.CategoryId==categoryId.Value).ToList();
            }
            if(!string.IsNullOrEmpty(q))
            {
                products=products.Where(p=>p.Name.ToLower().Contains(q.ToLower())).ToList();
            }
            int pageSize = 6;
            int pageNumber = page ?? 1;
            var vm = new ProductListVM
            {
                Products = products.Where(p=>p.IsActive).ToPagedList(pageNumber,pageSize),
                Categories = categories,
                CurrentCategoryId = categoryId,
                CurrentSearchQuery = q
            };
            return View(vm);
        }
        public async Task<IActionResult> Details(int id)
        {
            var entity = await uow.products.GetByIdAsync(id);
            if (entity == null || !entity.IsActive)
            {
                return NotFound();
            }
            var vm = new ProductVM
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Price = entity.Price,
                SKU = entity.SKU,
                StockQuantity = entity.StockQuantity,
                CategoryName = entity.Category?.Name,
                ImageUrl = entity.ImageUrl
            };

            return View(vm);
        }
    }
}