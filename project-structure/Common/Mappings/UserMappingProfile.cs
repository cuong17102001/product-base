using AutoMapper;
using Common.DTOs;
using Common.Models;

namespace Common.Mappings;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UpdateUserDto, User>();
    }
} 