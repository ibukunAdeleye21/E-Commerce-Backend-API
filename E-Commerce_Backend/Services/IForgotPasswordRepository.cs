using E_Commerce_Backend.Entity;

namespace E_Commerce_Backend.Services
{
    public interface IForgotPasswordRepository
    {
        Task<User?> UserEmailExistAsync(string email);
        Task UpdateUserAsync(User userDetail);
        Task<bool> SaveChangesAsync();
    }
}
