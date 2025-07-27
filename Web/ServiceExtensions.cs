using App.Interfaces.Repos;
using App.Interfaces.Repos.Generic;
using Infrastructure.Repos;
using Infrastructure.Repos.Generic;

namespace Web
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Agrega las dependencias relacionadas con los repositorios.
        /// </summary>
        /// <param name="services">Colección de servicios.</param>
        /// <returns>La colección de servicios actualizada.</returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            // Registro de repositorios genéricos
            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            
            // Registro de repositorios específicos
            services.AddScoped<IClienteRepo, ClienteRepo>();
            
            return services;
        }
    }
}
