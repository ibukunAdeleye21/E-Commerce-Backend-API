using E_Commerce_Backend.Entity;

namespace E_Commerce_Backend.Services
{
    public interface IOrderRepository
    {
        Task<User?> GetUserCartAsync(int id);
        Task AddOrderAsync(Order order);
        Task<bool> SaveChangesAsync();
        Task<IEnumerable<Order?>> GetOrderAsync(int id);
        Task UpdateCartAsync(Cart cart);
        Task<Cart?> GetIsActiveCartAsync(int userId);
    }
}
