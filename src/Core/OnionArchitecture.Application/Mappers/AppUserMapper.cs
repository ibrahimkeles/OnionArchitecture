using AutoMapper;
using OnionArchitecture.Application.Features.Commands.User.RegisterUser;
using OnionArchitecture.Domain.Identity;
using YourCoach.Application.DTOS.Token;

namespace OnionArchitecture.Application.Mappers
{
    public class AppUserMapper : Profile
    {
        public AppUserMapper()
        {
            CreateMap<AppUser, RegisterUserRequest>().ReverseMap();
            CreateMap<AppUser, TokenUser>().ReverseMap();
        }
    }
}
