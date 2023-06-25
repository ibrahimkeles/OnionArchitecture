using Microsoft.Extensions.DependencyInjection;
using OnionArchitecture.Application.Infrastructure.TokenServices;
using OnionArchitecture.Infrastructure.Services.TokenServices;

namespace OnionArchitecture.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<ITokenService, TokenService>();
        }
    }
}
