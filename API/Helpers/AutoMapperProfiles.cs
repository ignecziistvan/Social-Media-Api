using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            .ForMember(d => d.UserName, o => o.MapFrom(s => s.User.UserName));
    }
}
