using AutoMapper;
using Entities;
using DTO;

namespace MyShop
{
    public class Mapper:Profile
    {
        public Mapper()
        {
            CreateMap<Category, CategoryDTO>();
            CreateMap<Product, ProductDTO>();
            CreateMap<Order, OrderDTO>();
            CreateMap<User, UserDTO>();
            CreateMap<PostUserDTO,User >();
        }

    }
}
