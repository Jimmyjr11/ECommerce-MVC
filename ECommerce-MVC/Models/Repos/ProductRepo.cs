using ECommerce_MVC.Models.Data;
using ECommerce_MVC.Models.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ECommerce_MVC.Models.Repos
{
    public class ProductRepo : IProductRepo
    {
        private readonly ECommerceContext context;
        public ProductRepo(ECommerceContext _context)
        {
            context = _context;
        }
        public async Task AddAsync(Product entity)
        {
            await context.Products.AddAsync(entity);
        }

        public void Delete(int id)
        {
            var entity = context.Products.Find(id);
            if (entity != null)
            {
                context.Products.Remove(entity);
            }
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await context.Products.Include(c=>c.Category).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await context.Products.Include(c=>c.Category).FirstOrDefaultAsync(p=>p.ProductId==id);
        }

        public void Update(Product entity)
        {
            context.Products.Update(entity);
        }
    }
}
