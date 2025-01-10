using E_Commerce_Backend.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Services
{
    public class ValidateResetGuidRepository : IValidateResetGuidRepository
    {
        private readonly Context _context;
        public ValidateResetGuidRepository(Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> ValidateResetGuidAsync(string guid)
        {
            return await _context.Users.AnyAsync(
                c => c.PasswordResetGuid == guid 
                && c.IsUsed == false 
                && c.PasswordResetGuidExpiry >= DateTime.UtcNow);
        }
    }
}
