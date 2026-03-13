using ECommerce_MVC.Models.Repos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Text.Json;
using ECommerce_MVC.Models.VIEWMODELS;

namespace ECommerce_MVC.Controllers
{
    public class CartController : Controller
    {
        IUnitOfWork uow;
        public CartController(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId, int quantity = 1)
        {
            var product = await uow.products.GetByIdAsync(productId);
            if (product == null)
            {
                return NotFound();
            }

            List<CartItemVM> cartList;
            var cart = HttpContext.Session.GetString("ShoppingCart");
            if (cart == null)
            {
                cartList = new List<CartItemVM>();
            }
            else
            {
                cartList = JsonSerializer.Deserialize<List<CartItemVM>>(cart);
            }

            if (cartList.Any(c => c.ProductId == productId))
            {
                var item = cartList.FirstOrDefault(c => c.ProductId == productId);
                item.Quantity += quantity;
            }
            else
            {
                cartList.Add(new CartItemVM
                {
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    Price = product.Price,
                    Quantity = quantity
                });
            }

            HttpContext.Session.SetString("ShoppingCart", JsonSerializer.Serialize(cartList));

            string returnUrl = Request.Headers["Referer"].ToString();
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = true, count = cartList.Sum(x => x.Quantity) });
            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var cartJson = HttpContext.Session.GetString("ShoppingCart");
            List<CartItemVM> items;

            if (string.IsNullOrEmpty(cartJson))
            {
                items = new List<CartItemVM>();
            }
            else
            {
                items = JsonSerializer.Deserialize<List<CartItemVM>>(cartJson);
            }

            var cartVM = new Cart
            {
                CartItems = items
            };

            return View(cartVM);
        }

        public IActionResult Remove(int id)
        {
            var cartJson = HttpContext.Session.GetString("ShoppingCart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return RedirectToAction("Index");
            }
            var cartList = JsonSerializer.Deserialize<List<CartItemVM>>(cartJson);
            var itemToRemove = cartList.FirstOrDefault(c => c.ProductId == id);
            if (itemToRemove != null)
            {
                cartList.Remove(itemToRemove);
                HttpContext.Session.SetString("ShoppingCart", JsonSerializer.Serialize(cartList));
            }
            return RedirectToAction("Index");
        }

        public IActionResult Clear()
        {
            HttpContext.Session.Remove("ShoppingCart");
            return RedirectToAction("Index");
        }

        public IActionResult Decrease(int id)
        {
            var cartJson = HttpContext.Session.GetString("ShoppingCart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return RedirectToAction("Index");
            }
            var cartList = JsonSerializer.Deserialize<List<CartItemVM>>(cartJson);
            var item = cartList.FirstOrDefault(c => c.ProductId == id);

            if (item != null)
            {
                if (item.Quantity > 1)
                {
                    item.Quantity--;
                }
                else
                {
                    cartList.Remove(item);
                }

                HttpContext.Session.SetString("ShoppingCart", JsonSerializer.Serialize(cartList));
            }

            return RedirectToAction("Index");
        }
    }
}