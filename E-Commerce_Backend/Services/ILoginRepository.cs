using E_Commerce_Backend.Entity;

namespace E_Commerce_Backend.Services
{
    public interface ILoginRepository
    {
        Task<User?> GetUserByEmail(string email);
    }
}
