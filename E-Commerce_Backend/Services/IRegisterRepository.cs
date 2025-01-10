using E_Commerce_Backend.Entity;
using E_Commerce_Backend.Model;

namespace E_Commerce_Backend.Services
{
  public interface IRegisterRepository
  {
    Task<bool> SaveChangesAsync();
    Task<UserExistResultDto> UserExistAsync(User user);
    Task RegisterUserAsync(User user);

  }
}
