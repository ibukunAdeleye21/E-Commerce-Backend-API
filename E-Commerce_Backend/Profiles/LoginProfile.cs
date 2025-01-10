using AutoMapper;

namespace E_Commerce_Backend.Profiles
{
    public class LoginProfile : Profile
    {
        public LoginProfile()
        {
            CreateMap<Entity.User, Model.UserModelDto>();
        }
    }
}
