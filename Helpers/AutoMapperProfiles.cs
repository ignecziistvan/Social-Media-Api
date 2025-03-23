using API.Dtos.Request;
using API.Dtos.Response;
using API.Entities;
using AutoMapper;

namespace API.Helpers;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {
        CreateMap<User, UserDto>()
            .ForMember(d => d.MainPhoto, o => o.MapFrom(s => 
                s.Photos.FirstOrDefault(p => p.IsMain)
            )
        );
        
        CreateMap<RegistrationDto, User>();

        CreateMap<User, AccountDto>()
            .ForMember(d => d.MainPhoto, o => o.MapFrom(s => 
                s.Photos.FirstOrDefault(p => p.IsMain)
            )
        );

        CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));

        CreateMap<Post, PostDto>()
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.UserName))
            .ForMember(d => d.Likes, o => o.MapFrom(s => s.LikedByUsers))
            .ForMember(d => d.CommentCount, o => o.MapFrom(s => s.Comments.Count))
            .ForMember(d => d.UserMainPhoto, o => o.MapFrom(s => s.User.Photos.FirstOrDefault(p => p.IsMain)));

        CreateMap<Comment, CommentDto>()
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.UserName))
            .ForMember(d => d.UserMainPhoto, o => o.MapFrom(s => s.User.Photos.FirstOrDefault(p => p.IsMain)));

        CreateMap<Like, LikeDto>()
            .ForMember(d => d.UserMainPhoto, o => o.MapFrom(s => s.User.Photos.FirstOrDefault(p => p.IsMain)));

        CreateMap<Message, MessageDto>()
            .ForMember(d => d.ReceiverMainPhoto, o => o.MapFrom(s => s.Receiver.Photos.FirstOrDefault(p => p.IsMain)))
            .ForMember(d => d.SenderMainPhoto, o => o.MapFrom(s => s.Sender.Photos.FirstOrDefault(p => p.IsMain)));

        CreateMap<Photo, PhotoDto>();

        CreateMap<ProfilePhoto, PhotoDto>();

        CreateMap<DateTime, DateTime>()
            .ConvertUsing(d => DateTime.SpecifyKind(d, DateTimeKind.Utc));

        CreateMap<DateTime?, DateTime?>().ConvertUsing(d => d.HasValue 
            ? DateTime.SpecifyKind(d.Value, DateTimeKind.Utc) : null);
    }
}
