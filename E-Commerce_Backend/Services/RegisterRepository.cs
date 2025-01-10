using E_Commerce_Backend.DbContexts;
using E_Commerce_Backend.Entity;
using E_Commerce_Backend.Model;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_Backend.Services
{
  public class RegisterRepository : IRegisterRepository
  {
    private readonly Context _context;

    public RegisterRepository(Context context)
    {
      _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    public async Task<bool> SaveChangesAsync()
    {
      return (await _context.SaveChangesAsync() >= 0);
    }

    public async Task<UserExistResultDto> UserExistAsync(User user)
    {
      var result = new UserExistResultDto
      {
        EmailExists = await _context.Users.AnyAsync(p => p.Email == user.Email),
        PhonenumberExists = await _context.Users.AnyAsync(p => p.Phonenumber == user.Phonenumber)
      };

      return result;
    }

    public async Task RegisterUserAsync(User user)
    {
      await _context.Users.AddAsync(user);
      await _context.SaveChangesAsync();
    }
  }
}
