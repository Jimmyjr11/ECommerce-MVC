namespace ECommerce_MVC.Models.Repos
{
    public interface IEntityRepo<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}