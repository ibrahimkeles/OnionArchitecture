using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OnionArchitecture.Application.Infrastructure.TokenServices;
using OnionArchitecture.Infrastructure.Services.TokenServices.Extensions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YourCoach.Application.DTOS.Token;

namespace OnionArchitecture.Infrastructure.Services.TokenServices
{
    public class TokenService : ITokenService
    {
        private IConfiguration _configuration;
        private DateTime _accessTokenExpiration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public AccessToken CreateToken(TokenUser user)
        {
            _accessTokenExpiration = DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["Token:AccessTokenExpiration"]));
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            JwtSecurityToken jwtSecurityToken = CreateJwtSecurityToken(user, signingCredentials);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            string token = tokenHandler.WriteToken(jwtSecurityToken);
            return new AccessToken
            {
                Token = token,
                Expiration = _accessTokenExpiration
            };
        }
        public JwtSecurityToken CreateJwtSecurityToken(TokenUser user,
        SigningCredentials signingCredentials)
        {
            var jwt = new JwtSecurityToken(
                issuer: _configuration["Token:Issuer"],
                audience: _configuration["Token:Audience"],
                expires: _accessTokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user),
                signingCredentials: signingCredentials
            );
            return jwt;
        }
        private IEnumerable<Claim> SetClaims(TokenUser user)
        {
            var claims = new List<Claim>();
            claims.AddId(user.Id);
            claims.AddFirstName(user.FirstName);
            claims.AddLastName(user.LastName);
            claims.AddEmail(user.Email);
            return claims;
        }
    }
}
