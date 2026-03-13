// IOrderRepo.cs
using ECommerce_MVC.Models.Model;
namespace ECommerce_MVC.Models.Repos
{
    public interface IOrderRepo : IEntityRepo<Order>
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
    }
}
namespace ECommerce_MVC.Models.Repos
{
    public interface IOrderItemRepo : IEntityRepo<OrderItem> { }
}