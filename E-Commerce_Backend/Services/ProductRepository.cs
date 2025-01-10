using E_Commerce_Backend.DbContexts;
using E_Commerce_Backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly Context _context;
        public ProductRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<AllProduct>> GetProductsAsync()
        {
            return await _context.AllProducts
                .Include(c => c.Rating)
                .OrderBy(c => c.Id).ToListAsync();
        }

        public async Task<AllProduct?> GetProductAsync(int id)
        {
            return await _context.AllProducts
                .Include(c => c.Rating)
                .FirstOrDefaultAsync(ci => ci.Id == id);
        }
    }
}
