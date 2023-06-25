using Microsoft.Extensions.DependencyInjection;
using OnionArchitecture.Application.Repositories;
using OnionArchitecture.Persistance.Repositories;

namespace OnionArchitecture.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistanceServices(this IServiceCollection services)
        {
            services.AddScoped<ITodoListRepository, TodoListRepository>();
        }
    }
}
