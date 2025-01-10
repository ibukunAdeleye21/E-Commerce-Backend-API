using E_Commerce_Backend.DbContexts;
using E_Commerce_Backend.Entity;
using E_Commerce_Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Services
{
    public class CartRepository : ICartRepository
    {
        private readonly Context _context;
        public CartRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<Cart?> UserActiveCartAsync(int userId)
        {
            return await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive == true);
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _context.AddAsync(cart);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Update(cartItem);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> CartExistAsync(int id)
        {
            return await _context.Carts
                .AnyAsync(c => c.UserId == id);
        }

        public async Task<Cart?> GetCartWithoutCartItemAsync(int userId)
        {
            return await _context.Carts
                .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive == true);
        }

        public async Task<bool> CartIsActiveAsync(int id)
        {
            return await _context.Carts
                .Where(c => c.UserId == id)
                .Select(c => c.IsActive) // Assuming isActive is a bool field in the Cart entity
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CartItem>> GetCartItemsForCartAsync(int id)
        {
            return await _context.CartItems.Where(c => c.CartId == id).ToListAsync();
        }

        public async void RemoveCartItemForCartAsync(CartItem cartItemEntity)
        {
            _context.CartItems.Remove(cartItemEntity);
        }

        public void RemoveAllCartItemForCartAsync(int cartId)
        {
            var cartItemsForCart = _context.CartItems.Where(ci => ci.CartId == cartId);

            _context.CartItems.RemoveRange(cartItemsForCart);;

        }

        public async Task<IEnumerable<CartItemWithProductDetailsDto>> GetCartItemWithProductDetailsAsync(int id)
        {
            return await _context.CartItems
                .Where(c => c.CartId == id)
                .Select(ci => new CartItemWithProductDetailsDto
                {
                    Id = ci.Id,
                    Quantity = ci.Quantity,
                    Price = ci.Price,
                    Amount = ci.Amount,
                    ProductId = ci.AllProductId,
                    ProductTitle = ci.AllProduct.Title,
                    ProductCategory = ci.AllProduct.Category,
                    ProductDescription = ci.AllProduct.Description,
                    ProductImage = ci.AllProduct.Image,
                    ProductRate = ci.AllProduct.Rating.Rate,
                    ProductCount = ci.AllProduct.Rating.Count
                })
                .ToListAsync();
        }
    }
}
