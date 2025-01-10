using E_Commerce_Backend.Entity;

namespace E_Commerce_Backend.Services
{
    public interface IResetPasswordRepository
    {
        Task<User?> IsGuidValidAsync(string guid);
        Task UpdateUserAsync(User resetRequest);
        Task<bool> SaveChangesAsync();
    }
}
