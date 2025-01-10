using AutoMapper;

namespace E_Commerce_Backend.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Entity.CartItem, Model.CartItemDto>();
        }
    }
}

