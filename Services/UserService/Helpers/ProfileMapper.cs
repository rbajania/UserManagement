using AutoMapper;
using Events;
using UserService.DTOs;

namespace UserService;
public class ProfileMapper : Profile
{
    public ProfileMapper()
    {
        CreateMap<User, UserDto>();

        CreateMap<CreateUserDto, User>();

        CreateMap<UserDto, UserCreated>();

        CreateMap<User, UserUpdated>();
    }
}