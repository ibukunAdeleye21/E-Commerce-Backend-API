using E_Commerce_Backend.DbContexts;
using E_Commerce_Backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Services
{
    public class LoginRepository : ILoginRepository
    {
        private readonly Context _context;
        public LoginRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<User?> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(p => p.Email == email);
        }
    }
}
