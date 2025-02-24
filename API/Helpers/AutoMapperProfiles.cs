using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
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
    }
}
