using YourCoach.Application.DTOS.Token;

namespace OnionArchitecture.Application.Infrastructure.TokenServices
{
    public interface ITokenService
    {
        AccessToken CreateToken(TokenUser user);
    }
}
