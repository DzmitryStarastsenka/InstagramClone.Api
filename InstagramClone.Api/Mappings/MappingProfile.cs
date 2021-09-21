using AutoMapper;
using InstagramClone.Application.Helpers;
using InstagramClone.Application.Models.Authentificate;
using InstagramClone.Application.Models.User;
using InstagramClone.Domain.DAL.Models.Post;
using InstagramClone.Domain.DAL.Models.User;

namespace InstagramClone.Api.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Source -> Target
            CreateMap<UserProfileDTO, UserProfile>().ReverseMap();
            CreateMap<RegisterRequest, UserProfileDTO>();
            CreateMap<UpdateUserRequest, UserProfileDTO>();
        }
    }
}