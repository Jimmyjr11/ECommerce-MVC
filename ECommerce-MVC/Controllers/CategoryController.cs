using ECommerce_MVC.Models.Repos;
using Microsoft.AspNetCore.Mvc;
using ECommerce_MVC.Models.VIEWMODELS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using ECommerce_MVC.Models.Model;
namespace ECommerce_MVC.Controllers
{
    public class CategoryController : Controller
    {
        IUnitOfWork uow;
        public CategoryController(IUnitOfWork uow)
        {
            this.uow = uow;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await uow.Categories.GetCategoriesWithParentsAsync();

            var vm = categories.Select(c => new CategoryVM
            {
                CategoryId = c.CategoryId,
                CategoryName = c.Name,
                ParentCategoryId = c.ParentCategoryId,
                ParentCategoryName = c.ParentCategory?.Name
            }).ToList();

            return View(vm);
        }
        public async Task<IActionResult> create()
        {
            var categories = await uow.Categories.GetAllAsync();
            ViewBag.Categories = categories;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CategoryVM vm)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = vm.CategoryName,
                    ParentCategoryId = vm.ParentCategoryId
                };
                await uow.Categories.AddAsync(category);
                await uow.CompleteAsync();
                return RedirectToAction("Index");
            }
            var categories = await uow.Categories.GetAllAsync();
            ViewBag.Categories = categories;
            return View(vm);
        }
        public async Task<ActionResult> Edit(int id)
        {
            var category = await uow.Categories.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var vm = new CategoryVM
            {
                CategoryId = category.CategoryId,
                ParentCategoryId = category.ParentCategoryId,
                CategoryName = category.Name,
            };
            var CategoryList = await uow.Categories.GetAllAsync();
            ViewBag.Categories = CategoryList.Where(category => category.CategoryId != id).ToList();
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CategoryVM vm)
        {
            if (ModelState.IsValid)
            {
                var category = await uow.Categories.GetByIdAsync(vm.CategoryId);
                if (category == null) return NotFound();

                category.Name = vm.CategoryName;
                category.ParentCategoryId = vm.ParentCategoryId;

                uow.Categories.Update(category);
                await uow.CompleteAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(vm);
        }
        public async Task<ActionResult> Delete(int id)
        {
            var category = await uow.Categories.GetByIdAsync(id);
            if (category == null) return NotFound();
            uow.Categories.Delete(id);
            await uow.CompleteAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<ActionResult> Details(int id)
        {
            var category= await uow.Categories.GetByIdAsync((int)id);
            if (category == null) return NotFound();
            var vm = new CategoryVM
            {
                CategoryId = category.CategoryId,
                CategoryName = category.Name,
                ParentCategoryName = category.ParentCategory?.Name
            };
            return View(vm);
        }

    }
    }
