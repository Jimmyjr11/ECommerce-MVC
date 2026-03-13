namespace ECommerce_MVC.Models.Repos
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepo Categories { get; }
        IProductRepo products { get; }
        IOrderRepo Orders { get; }
        IOrderItemRepo OrderItems { get; }
        Task<int> CompleteAsync(); // This is the single SaveChanges call
    }
}