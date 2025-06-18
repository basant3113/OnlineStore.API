using AutoMapper;
using OnlineStore.API.Core.Models;
using OnlineStore.API.DAL.Models;
using OnlineStore.API.PL.DTOs;

namespace OnlineStore.API.PL.Mapping
{
    public class ApplicationProfile:Profile
    {
        public ApplicationProfile(IConfiguration configuration)
        {
            CreateMap<Product , ProductDTO>().ReverseMap();
            CreateMap<ApplicationUser , RegisterDTO>().ReverseMap();
            CreateMap<ApplicationUser , UpdateUserDTO>().ReverseMap();
            CreateMap<Comment , CommentDTO>().ReverseMap();
            CreateMap<Comment, CommentReturnDTO>()
                .ForMember(src => src.UserName, m => m.MapFrom(c => c.User.UserName));
            CreateMap<ProductType , TypeDTO>().ReverseMap();
            CreateMap<Favourite , FavouriteDTO>().ReverseMap();
            
        }
    }
}
