using API.Dtos.Request;
using API.Dtos.Response;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, UserDto>();
        CreateMap<RegistrationDto, User>();
        CreateMap<User, AccountDto>();
        CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));
        CreateMap<Post, PostDto>()
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.UserName))
            .ForMember(d => d.Likes, o => o.MapFrom(s => s.LikedByUsers))
            .ForMember(d => d.CommentCount, o => o.MapFrom(s => s.Comments.Count));
        CreateMap<Comment, CommentDto>()
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.UserName));
        CreateMap<Like, LikeDto>();
        CreateMap<Message, MessageDto>();
        CreateMap<DateTime, DateTime>()
            .ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));
        CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue 
            ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
    }
}
