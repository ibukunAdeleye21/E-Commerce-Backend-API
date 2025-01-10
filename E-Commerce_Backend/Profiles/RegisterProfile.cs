using AutoMapper;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace E_Commerce_Backend.Profiles
{
  public class RegisterProfile : Profile
  {
    public RegisterProfile()
    {
      CreateMap<Model.RegisterDto, Entity.User>();
    }
  }
}
