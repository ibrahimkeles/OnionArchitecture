using AutoMapper;
using OnionArchitecture.Application.Features.Commands.RegisterStudent;
using OnionArchitecture.Domain.Identity;

namespace OnionArchitecture.Application.Mappers
{
    public class AppUserMapper : Profile
    {
        public AppUserMapper()
        {
            CreateMap<AppUser, RegisterUserRequest>().ReverseMap();
        }
    }
}
