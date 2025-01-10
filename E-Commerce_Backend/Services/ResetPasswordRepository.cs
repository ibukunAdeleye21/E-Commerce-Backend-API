using E_Commerce_Backend.DbContexts;
using E_Commerce_Backend.Entity;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Services
{
    public class ResetPasswordRepository : IResetPasswordRepository
    {
        private readonly Context _context;
        public ResetPasswordRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<User?> IsGuidValidAsync(string guid)
        {
            return await _context.Users.FirstOrDefaultAsync(c => c.PasswordResetGuid == guid);
        }

        public async Task UpdateUserAsync(User resetRequest)
        {
            _context.Users.Update(resetRequest);

            await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
