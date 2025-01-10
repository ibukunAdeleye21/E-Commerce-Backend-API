using E_Commerce_Backend.DbContexts;
using E_Commerce_Backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Services
{
    public class ChangePasswordRepository : IChangePasswordRepository
    {
        private readonly Context _context;
        public ChangePasswordRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User?> GetUserByIdAndEmailAsync(int id, string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(c => c.Id == id && c.Email == email);
        }

        public async Task UpdateUserAsync(User userDetails)
        {
            _context.Users.Update(userDetails);

            await _context.SaveChangesAsync();
        }
    }
}
