using ECommerce_MVC.Models.Model;

namespace ECommerce_MVC.Models.Repos
{
    public interface ICategoryRepo : IEntityRepo<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesWithParentsAsync();
    }
}