using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OnionArchitecture.Application;
using OnionArchitecture.Domain.Identity;
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
            builder.Services.AddIdentity<AppUser, AppRole>(_ =>
            {
                _.Password.RequiredLength = 5; //En az ka� karakterli olmas� gerekti�ini belirtiyoruz.
                _.Password.RequireNonAlphanumeric = false; //Alfanumerik zorunlulu�unu kald�r�yoruz.
                _.Password.RequireLowercase = false; //K���k harf zorunlulu�unu kald�r�yoruz.
                _.Password.RequireUppercase = false; //B�y�k harf zorunlulu�unu kald�r�yoruz.
                _.Password.RequireDigit = false; //0-9 aras� say�sal karakter zorunlulu�unu kald�r�yoruz.   
            })
                .AddEntityFrameworkStores<TodoDbContext>();
            builder.Services.AddApplicationServices();
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

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.UseMiddleware<GlobalException>();
            app.Run();
        }
    }
}