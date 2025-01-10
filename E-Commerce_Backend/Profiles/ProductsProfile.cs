using AutoMapper;
using E_Commerce_Backend.Entity;
using E_Commerce_Backend.Model;

namespace E_Commerce_Backend.Profiles
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile()
        {
            CreateMap<Entity.AllProduct, Model.ProductDto>();

            CreateMap<Entity.Rating, Model.RatingDto>();
        }
    }
}
