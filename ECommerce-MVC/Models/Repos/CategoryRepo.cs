using ECommerce_MVC.Models.Data;
using ECommerce_MVC.Models.Model;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ECommerce_MVC.Models.Repos
{
    public class CategoryRepo : ICategoryRepo
    {
        private readonly ECommerceContext context;
        public CategoryRepo(ECommerceContext _context)
        {
            context = _context;
        }

        public async Task AddAsync(Category entity)
        {
            await context.Categories.AddAsync(entity);
        }

        public void Delete(int id)
        {
            var entity =  context.Categories.Find(id);
            if (entity != null)
            {
                context.Categories.Remove(entity);
            }
        }

        // Updated: Use Include so the Index page always has the parent name
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await context.Categories
                .Include(c => c.ParentCategory)
                .ToListAsync();
        }

        // UPDATED BUG FIX: FindAsync does not support .Include()
        // We use FirstOrDefaultAsync so the Details page can see the Parent Category Name
        public async Task<Category?> GetByIdAsync(int id)
        {
            return await context.Categories
                .Include(c => c.ParentCategory)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithParentsAsync()
        {
            return await context.Categories
                .Include(c => c.ParentCategory)
                .ToListAsync();
        }

        public void Update(Category entity)
        {
            context.Categories.Update(entity);
        }
    }
}