
using Microsoft.EntityFrameworkCore;
using OnionArchitecture.Domain.Identity;
using OnionArchitecture.Persistance.Contexts;

namespace OnionArchitecture.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<TodoDbContext>(_ => _.UseSqlServer(builder.Configuration.GetConnectionString("DB")));
            builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<TodoDbContext>();
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
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}