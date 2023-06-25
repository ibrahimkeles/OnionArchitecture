using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using OnionArchitecture.Domain.Identity;
using YourCoach.Application.Utils.Results;

namespace OnionArchitecture.Application.Features.Commands.RegisterStudent
{
    public class RegisterUserRequest : IRequest<Result>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class RegisterUserHandler : IRequestHandler<RegisterUserRequest, Result>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public RegisterUserHandler(UserManager<AppUser> userManager, IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<Result> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
        {
            string userName = request.FirstName.Trim() + " " + request.LastName.Trim();
            AppUser user = new();
            userName = userName.ToLower();
            userName = userName.Replace(" ", ".");
            user.UserName = userName;
            _mapper.Map(request, user);
            IdentityResult result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
                return new Result(true, "Kullanıcı kayıdı başarılı!");
            else
                return new Result(false, $"Hata : {result.Errors.FirstOrDefault()?.Description}");
        }
    }
    public class RegisterUserValidator : AbstractValidator<RegisterUserRequest>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("E-posta adresi boş geçilemez!");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Geçersiz e-posta adresi");
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("Ad alanı boş geçilemez!");
            RuleFor(x => x.LastName).NotEmpty().WithMessage("Soyad alanı boş geçilemez!");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Şifre alanı boş geçilemez!");
        }
    }
}
