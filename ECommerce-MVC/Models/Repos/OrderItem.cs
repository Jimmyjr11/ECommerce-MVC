using ECommerce_MVC.Models.Data;
using ECommerce_MVC.Models.Model;
using Microsoft.EntityFrameworkCore; // <--- This fixes the Include and Async errors!
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ECommerce_MVC.Models.Repos
{
    public class OrderItemRepo : IOrderItemRepo
    {
        private readonly ECommerceContext context;

        public OrderItemRepo(ECommerceContext _context)
        {
            context = _context;
        }

        public async Task AddAsync(OrderItem entity)
        {
            await context.OrderItems.AddAsync(entity);
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync()
        {
            return await context.OrderItems.Include(i => i.Product).ToListAsync();
        }

        public async Task<OrderItem?> GetByIdAsync(int id)
        {
            return await context.OrderItems.Include(i => i.Product).FirstOrDefaultAsync(i => i.OrderItemId == id);
        }

        public void Update(OrderItem entity)
        {
            context.OrderItems.Update(entity);
        }

        public void Delete(int id)
        {
            var entity = context.OrderItems.Find(id);
            if (entity != null)
            {
                context.OrderItems.Remove(entity);
            }
        }
    }
}