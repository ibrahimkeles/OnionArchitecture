using Microsoft.Extensions.DependencyInjection;

namespace OnionArchitecture.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining(typeof(ServiceRegistration)));
            services.AddAutoMapper(typeof(ServiceRegistration));
        }
    }
}
