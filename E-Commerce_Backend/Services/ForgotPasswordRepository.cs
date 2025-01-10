using E_Commerce_Backend.DbContexts;
using E_Commerce_Backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Services
{
    public class ForgotPasswordRepository : IForgotPasswordRepository
    {
        private readonly Context _context;
        public ForgotPasswordRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User?> UserEmailExistAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task UpdateUserAsync(User userDetails)
        {
            _context.Users.Update(userDetails);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }

    }
}
