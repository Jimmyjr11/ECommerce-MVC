using ECommerce_MVC.Models.Model;
using ECommerce_MVC.Models.Repos;
using ECommerce_MVC.Models.VIEWMODELS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce_MVC.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        IUnitOfWork uow;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(IUnitOfWork uow, UserManager<AppUser> userManager)
        {
            this.uow = uow;
            _userManager = userManager;
        }

        public ActionResult CheckOut()
        {
            var cartJson = HttpContext.Session.GetString("ShoppingCart");
            if (string.IsNullOrEmpty(cartJson))
            {
                return RedirectToAction("Index", "Cart");
            }
            var cartItems = System.Text.Json.JsonSerializer.Deserialize<List<CartItemVM>>(cartJson);
            var checkoutVM = new Models.VIEWMODELS.CheckoutVM
            {
                CartItems = cartItems,
                GrandTotal = cartItems.Sum(c => c.Price * c.Quantity)
            };
            return View(checkoutVM);
        }

        [HttpPost]
        public async Task<ActionResult> CheckOut(CheckoutVM vm)
        {
            ModelState.Remove("cartItems");
            ModelState.Remove("grandTotal");

            var cartjson = HttpContext.Session.GetString("ShoppingCart");
            var items = System.Text.Json.JsonSerializer.Deserialize<List<CartItemVM>>(cartjson);

            if (!ModelState.IsValid)
            {
                vm.CartItems = items;
                vm.GrandTotal = items.Sum(c => c.Price * c.Quantity);
                return View(vm);
            }

            foreach (var item in items)
            {
                var product = await uow.products.GetByIdAsync(item.ProductId);

                if (product == null || product.StockQuantity < item.Quantity)
                {
                    int availableStock = product?.StockQuantity ?? 0;
                    ModelState.AddModelError("", $"Sorry, we only have {availableStock} of '{item.ProductName}' left in stock.");

                    vm.CartItems = items;
                    vm.GrandTotal = items.Sum(c => c.Price * c.Quantity);
                    return View(vm);
                }
            }

            string currentUserId = _userManager.GetUserId(User);

            var order = new Order
            {
                UserId = currentUserId,
                OrderDate = DateTime.Now,
                TotalAmount = items.Sum(c => c.Price * c.Quantity),
                Status = 1,
                OrderNumber = "ORD-" + DateTime.Now.ToString("yyyyMMdd-HHmmss"),
                ShippingAddress = new Address
                {
                    UserId = currentUserId,
                    Street = vm.Address,
                    City = vm.City,
                    Country = "Egypt",
                    Zip = "00000",
                    IsDefault = true
                }
            };

            await uow.Orders.AddAsync(order);
            await uow.CompleteAsync();

            foreach (var item in items)
            {
                var itemOrder = new OrderItem
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price
                };
                await uow.OrderItems.AddAsync(itemOrder);

                var productToUpdate = await uow.products.GetByIdAsync(item.ProductId);
                productToUpdate.StockQuantity -= item.Quantity;
                uow.products.Update(productToUpdate);
            }

            await uow.CompleteAsync();
            HttpContext.Session.Remove("ShoppingCart");

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }

        public async Task<IActionResult> MyOrders()
        {
            string currentUserId = _userManager.GetUserId(User);
            var userOrders = await uow.Orders.GetOrdersByUserIdAsync(currentUserId);
            return View(userOrders);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Manage()
        {
            var allOrders = await uow.Orders.GetAllAsync();
            var sortedOrders = allOrders.OrderByDescending(o => o.OrderDate).ToList();
            return View(sortedOrders);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Manage(int orderId, int newStatus)
        {
            var order = await uow.Orders.GetByIdAsync(orderId);

            if (order != null)
            {
                order.Status = newStatus;
                uow.Orders.Update(order);
                await uow.CompleteAsync();
            }

            return RedirectToAction(nameof(Manage));
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await uow.Orders.GetByIdAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            var currentUserId = _userManager.GetUserId(User);
            if (order.UserId != currentUserId && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            var vm = new OrderDetailsVM
            {
                OrderHeader = order,
                OrderItems = order.OrderItems
            };

            return View(vm);
        }
    }
}