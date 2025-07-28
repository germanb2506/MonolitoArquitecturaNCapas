using App.Dto;
using App.Interfaces.Repos;
using App.Interfaces.Repos.Generic;
using App.Interfaces.Services;
using App.Services;
using Domain.Entities;
using Infrastructure.Repos;
using Infrastructure.Repos.Generic;

namespace Web
{
    public static class ServiceExtensions
    {
        /// <summary>
        /// Agrega las dependencias relacionadas con los repositorios y servicios.
        /// </summary>
        /// <param name="services">Colección de servicios.</param>
        /// <returns>La colección de servicios actualizada.</returns>
        public static IServiceCollection AddServices(this IServiceCollection services)
        {


            // Registro de AutoMapper para los perfiles de mapeo
            services.AddAutoMapper(config =>
            {
                config.CreateMap<Cliente, ClienteDto>().ReverseMap();
            });
            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>)); // si aplicas Genérico
            // Registro de repositorios específicos
            services.AddScoped<IClienteRepo, ClienteRepo>();

            // Registro de servicios específicos
            services.AddScoped<IClienteService, ClienteService>();

            return services;
        }
    }
}
