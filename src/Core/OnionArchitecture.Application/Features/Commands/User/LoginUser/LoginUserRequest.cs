using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OnionArchitecture.Application.Infrastructure.TokenServices;
using OnionArchitecture.Domain.Identity;
using YourCoach.Application.DTOS.Token;
using YourCoach.Application.Utils.Results;

namespace OnionArchitecture.Application.Features.Commands.User.LoginUser
{
    public class LoginUserRequest : IRequest<Result>
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class LoginUserHandler : IRequestHandler<LoginUserRequest, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public LoginUserHandler(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<Result> Handle(LoginUserRequest request, CancellationToken cancellationToken)
        {
            AppUser? appUser = await _userManager.FindByEmailAsync(request.Email);
            if (appUser is null) return new Result(false, $"{request.Email} e-posta adresine ait bir kullanıcı bulunamadı!");
            bool checkPassword = await _userManager.CheckPasswordAsync(appUser, request.Password);
            if (!checkPassword) return new Result(false, "Şifre yanlış!");
            TokenUser tokenUser = new();
            _mapper.Map(appUser, tokenUser);
            return new Result(true, "Kullanıcı girişi başarılı", new
            {
                AccessToken = _tokenService.CreateToken(tokenUser),
                User = new
                {
                    tokenUser.Id,
                    tokenUser.FirstName,
                    tokenUser.LastName,
                    tokenUser.Email
                }
            });
        }
    }
    public class LoginUserValidator : AbstractValidator<LoginUserRequest>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta adresi boş geçilemez!");
            RuleFor(x => x.Email).EmailAddress().WithMessage("E-posta formatı geçersiz!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre alanı boş geçilemez!");
        }
    }
}
