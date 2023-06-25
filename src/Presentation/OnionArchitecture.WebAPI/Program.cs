using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OnionArchitecture.Application;
using OnionArchitecture.Application.Features.Commands.User.RegisterUser;
using OnionArchitecture.Domain.Identity;
using OnionArchitecture.Infrastructure;
using OnionArchitecture.Persistance;
using OnionArchitecture.Persistance.Contexts;
using OnionArchitecture.WebAPI.Extensions;
using System.Text;

namespace OnionArchitecture.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<TodoDbContext>(_ => _.UseSqlServer(builder.Configuration.GetConnectionString("DB")));
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices();
            builder.Services.AddPersistanceServices();
            builder.Services.AddIdentity<AppUser, AppRole>(_ =>
            {
                _.Password.RequiredLength = 5; //En az kaç karakterli olmasý gerektiðini belirtiyoruz.
                _.Password.RequireNonAlphanumeric = false; //Alfanumerik zorunluluðunu kaldýrýyoruz.
                _.Password.RequireLowercase = false; //Küçük harf zorunluluðunu kaldýrýyoruz.
                _.Password.RequireUppercase = false; //Büyük harf zorunluluðunu kaldýrýyoruz.
                _.Password.RequireDigit = false; //0-9 arasý sayýsal karakter zorunluluðunu kaldýrýyoruz.   
                _.User.RequireUniqueEmail = true; //Email adreslerini tekilleþtiriyoruz.
                _.User.AllowedUserNameCharacters = "abcçdefghiýjklmnoöpqrsþtuüvwxyzABCÇDEFGHIÝJKLMNOÖPQRSÞTUÜVWXYZ0123456789-._@+"; //Kullanýcý adýnda geçerli olan karakterleri belirtiyoruz.
            })
                            .AddEntityFrameworkStores<TodoDbContext>()
                            .AddDefaultTokenProviders();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                            .AddJwtBearer(options =>
                 {
                     options.TokenValidationParameters = new TokenValidationParameters
                     {
                         ValidateIssuer = true,
                         ValidateAudience = true,
                         ValidateLifetime = true,
                         ValidIssuer = builder.Configuration["Token:Issuer"],
                         ValidAudience = builder.Configuration["Token:Audience"],
                         ValidateIssuerSigningKey = true,
                         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"].ToString()))
                     };
                 });
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TodoApp WEB API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {{
                        new OpenApiSecurityScheme {
                        Reference= new OpenApiReference{
                        Type=ReferenceType.SecurityScheme,Id="Bearer"}},
                    new string[]{}
                      }
                });
            });
            builder.Services.AddControllers()
                .AddFluentValidation(opt => { opt.RegisterValidatorsFromAssemblyContaining<RegisterUserValidator>(); })
                .ConfigureApiBehaviorOptions(opt => { opt.SuppressModelStateInvalidFilter = true; });
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseAuthentication();
            app.MapControllers();
            app.UseMiddleware<GlobalException>();
            app.Run();
        }
    }
}