using E_Commerce_Backend.Entity;

namespace E_Commerce_Backend.Services
{
    public interface IProductRepository
    {
        Task<IEnumerable<AllProduct>> GetProductsAsync();
        Task<AllProduct?> GetProductAsync(int id);
    }
}
