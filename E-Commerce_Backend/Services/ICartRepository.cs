using E_Commerce_Backend.Entity;
using E_Commerce_Backend.Model;

namespace E_Commerce_Backend.Services
{
    public interface ICartRepository
    {
        Task<bool> SaveChangesAsync();
        Task<Cart?> UserActiveCartAsync(int id);
        Task AddCartAsync(Cart cart);
        Task UpdateCartItemAsync(CartItem cartItem);
        Task<bool> CartExistAsync(int id);
        Task<Cart?> GetCartWithoutCartItemAsync(int userId);
        Task<IEnumerable<CartItem>> GetCartItemsForCartAsync(int id);
        void RemoveCartItemForCartAsync(CartItem cartItemEntity);
        void RemoveAllCartItemForCartAsync(int cartId);
        Task<IEnumerable<CartItemWithProductDetailsDto>> GetCartItemWithProductDetailsAsync(int id);
        Task<bool> CartIsActiveAsync(int id);
    }
}
