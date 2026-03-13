using ECommerce_MVC.Models.Repos;
using ECommerce_MVC.Models.VIEWMODELS;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce_MVC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IUnitOfWork uow;

        // The Controller asks for the UnitOfWork here!
        public CatalogController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IActionResult> Index()
        {
            var products=await uow.products.GetAllAsync();
            var active=products.Where(p=>p.IsActive==true).ToList();


            return View(active);
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
                CategoryName = entity.Category?.Name
            };

            return View(vm);
        }
    }
}