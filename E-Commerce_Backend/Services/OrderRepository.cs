using E_Commerce_Backend.DbContexts;
using E_Commerce_Backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Services
{
    public class OrderRepository : IOrderRepository
    {
        private readonly Context _context;

        public OrderRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User?> GetUserCartAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Cart)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<Cart?> GetIsActiveCartAsync(int userId)
        {
            return await _context.Carts
                .Include(u => u.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive == true);
        }
        public async Task<IEnumerable<Order?>> GetOrderAsync(int id)
        {
            return await _context.Orders
                .Include(u => u.CartItems)
                .ThenInclude(u => u.AllProduct)
                .Where(u => u.UserId == id).ToListAsync();
        }
        public async Task AddOrderAsync(Order order)
        {
            await _context.AddAsync(order);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);

            await _context.SaveChangesAsync();
        }
    }
}
