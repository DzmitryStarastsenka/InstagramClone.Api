using AutoMapper;
using InstagramClone.Application.Helpers;
using InstagramClone.Application.Models.Authentificate;
using InstagramClone.Application.Models.Post;
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

            CreateMap<UserPostDTO, UserPost>()
                .ForMember(dest => dest.Photo, op => op.MapFrom(sc => FileConvertHelper.ConvertToBytes(sc.Photo)));
            CreateMap<UserPost, UserPostDTO>()
                .ForMember(dest => dest.Photo, op => op.Ignore());

            CreateMap<PostCommentDTO, PostComment>().ReverseMap();
            CreateMap<PostLikeDTO, PostLike>().ReverseMap();

            CreateMap<CreatePostRequest, UserPostDTO>();
            CreateMap<CreateCommentRequest, PostCommentDTO>();

            CreateMap<UpdatePostRequest, UserPostDTO>();
            CreateMap<UpdatePostPhotoRequest, UserPostDTO>();
            CreateMap<EditCommentRequest, UserPostDTO>();
        }
    }
}