using E_Commerce_Backend.Entity;

namespace E_Commerce_Backend.Services
{
    public interface IChangePasswordRepository
    {
        Task<User?> GetUserByIdAndEmailAsync(int id, string email);
        Task UpdateUserAsync(User userDetails);
    }
}
