using ECommerce_MVC.Models.Model;
using ECommerce_MVC.Models.Repos;
using ECommerce_MVC.Models.VIEWMODELS;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce_MVC.Controllers
{
    public class ProductController : Controller
    {
        IUnitOfWork uow;
        public ProductController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public async Task<IActionResult> Index()
        {
            var entity = await uow.products.GetAllAsync();
            var vm = entity.Select(p => new ProductVM
            {
                ProductId = p.ProductId,
                Name = p.Name,
                Price = p.Price,
                SKU = p.SKU,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                StockQuantity = p.StockQuantity,
                CategoryName = p.Category?.Name
            });
            return View(vm);
        }
        public async Task<IActionResult> Create()
        {
            var categories = await uow.Categories.GetAllAsync();
            ViewBag.Categories = categories;

            return View();
        }
        [HttpPost]
        public async Task<ActionResult>Create(ProductVM vm)
        {
            if(ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = vm.Name,
                    Price = vm.Price,
                    SKU = vm.SKU,
                    IsActive = vm.IsActive,
                    CategoryId = vm.CategoryId,
                    StockQuantity = vm.StockQuantity,
                    CreatedAt= DateTime.Now,
                };
                await uow.products.AddAsync(product);
                await uow.CompleteAsync();
                return RedirectToAction("index");
            }
            ViewBag.Categories = await uow.Categories.GetAllAsync();
            return View(vm);
        }
        public async Task<IActionResult> Edit(int id)
        {
            var entity = await uow.products.GetByIdAsync(id);

            if (entity == null)
            {
                return NotFound(); 
            }

            var vm = new ProductVM
            {
                ProductId = entity.ProductId, 
                Name = entity.Name,
                Price = entity.Price,
                SKU = entity.SKU,
                IsActive = entity.IsActive,
                CategoryId = entity.CategoryId,
                StockQuantity = entity.StockQuantity
            };

            ViewBag.Categories = await uow.Categories.GetAllAsync();

            return View(vm); 
        }
        [HttpPost]
        [HttpPost]
        public async Task<ActionResult> Edit(ProductVM vm)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    ProductId = vm.ProductId,
                    Name = vm.Name,
                    Price = vm.Price,
                    SKU = vm.SKU,
                    IsActive = vm.IsActive,
                    CategoryId = vm.CategoryId,
                    StockQuantity = vm.StockQuantity,
                };

                uow.products.Update(product);
                await uow.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await uow.Categories.GetAllAsync();
            return View(vm);
        }
        public async Task<ActionResult> details(int id)
        {
            var entity=await uow.products.GetByIdAsync(id);
            if(entity== null)
            {
                return NotFound();
            }
            var vm = new ProductVM
            {
                ProductId = entity.ProductId,
                Name = entity.Name,
                Price = entity.Price,
                SKU = entity.SKU,
                IsActive = entity.IsActive,
                CategoryId = entity.CategoryId,
                StockQuantity = entity.StockQuantity,
                CategoryName = entity.Category?.Name
            };
            return View(vm);
        }
        public async Task<ActionResult> Delete(int id)
        {
            var product = await uow.products.GetByIdAsync(id);
            if (product == null) return NotFound();
            uow.products.Delete(id);
            await uow.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}

