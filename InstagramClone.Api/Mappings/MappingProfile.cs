using AutoMapper;
using InstagramClone.Application.Models.Authentificate;
using InstagramClone.Application.Models.Post;
using InstagramClone.Application.Models.Post.Requests;
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
            CreateMap<UserProfileDto, UserProfile>().ReverseMap();

            //CreateMap<UserProfile, PostListItemDto>()
            //    .ForMember(dest => dest.LikesCount, op => op.MapFrom(src => src.Likes.Count))
            //    .ForMember(dest => dest.CommentsCount, op => op.MapFrom(src => src.Comments.Count));

            CreateMap<PostComment, PostCommentListItemDto>();

            CreateMap<RegisterRequest, UserProfileDto>();
            CreateMap<UpdateUserRequest, UserProfileDto>();

            CreateMap<CreatePostRequest, UserPost>();
            CreateMap<CreateCommentRequest, PostComment>();

            CreateMap<UpdatePostRequest, UserPost>();
            CreateMap<UpdateCommentRequest, PostComment>();
        }
    }
}