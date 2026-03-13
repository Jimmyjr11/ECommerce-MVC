using ECommerce_MVC.Controllers;
using ECommerce_MVC.Models.Data;
using ECommerce_MVC.Models.Model;
using Microsoft.AspNetCore.Identity;

namespace ECommerce_MVC.Models.Repos
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ECommerceContext _context;
        public ICategoryRepo Categories { get; private set; }

        public IProductRepo products { get; private set; }
        public IOrderRepo Orders { get; private set; }
        public IOrderItemRepo OrderItems { get; private set; }
        public UnitOfWork(ECommerceContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            Categories = new CategoryRepo(_context);
            products = new ProductRepo(_context);
            Orders = new OrderRepo(_context);
            OrderItems = new OrderItemRepo(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}