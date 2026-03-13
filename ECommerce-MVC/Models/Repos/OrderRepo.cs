using ECommerce_MVC.Models.Data;
using ECommerce_MVC.Models.Model;
using Microsoft.EntityFrameworkCore;

namespace ECommerce_MVC.Models.Repos
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ECommerceContext context;
        public OrderRepo(ECommerceContext _context)
        {
            context = _context;
        }

        public async Task AddAsync(Order entity)
        {
            await context.Orders.AddAsync(entity);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await context.Orders.Include(o => o.User).ToListAsync();
        }
        public async Task<Order?> GetByIdAsync(int id)
        {
            return await context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems) 
                .FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public void Update(Order entity)
        {
            context.Orders.Update(entity);
        }

        public void Delete(int id)
        {
            var entity = context.Orders.Find(id);
            if (entity != null) context.Orders.Remove(entity);
        }
        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await context.Orders
                .Include(o => o.OrderItems)           
                .ThenInclude(i => i.Product)          
                .Where(o => o.UserId == userId)       
                .OrderByDescending(o => o.OrderDate) 
                .ToListAsync();
        }
    }
}