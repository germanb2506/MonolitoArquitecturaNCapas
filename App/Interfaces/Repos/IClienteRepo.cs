using App.Interfaces.Repos.Generic;
using Domain.Entities;

namespace App.Interfaces.Repos
{
    /// <summary>
    /// Interfaz para el repositorio de Cliente que define operaciones específicas además de las genéricas.
    /// </summary>
    public interface IClienteRepo : IGenericRepo<Cliente>
    {
        /// <summary>
        /// Obtiene un cliente por su NIT.
        /// </summary>
        /// <param name="nit">Número de identificación tributaria</param>
        /// <returns>Cliente encontrado o null si no existe</returns>
        Task<Cliente> ObtenerPorNit(string nit);

        /// <summary>
        /// Obtiene un cliente por su correo electrónico.
        /// </summary>
        /// <param name="correo">Correo electrónico del cliente</param>
        /// <returns>Cliente encontrado o null si no existe</returns>
        Task<Cliente> ObtenerPorCorreo(string correo);

        /// <summary>
        /// Obtiene todos los clientes de una ciudad específica.
        /// </summary>
        /// <param name="ciudad">Nombre de la ciudad</param>
        /// <returns>Lista de clientes de la ciudad</returns>
        Task<List<Cliente>> ObtenerPorCiudad(string ciudad);

        /// <summary>
        /// Obtiene todos los clientes activos.
        /// </summary>
        /// <returns>Lista de clientes activos</returns>
        Task<List<Cliente>> ObtenerActivos();

        /// <summary>
        /// Obtiene todos los clientes de un tipo específico.
        /// </summary>
        /// <param name="tipoCliente">Tipo de cliente (Jurídica o Natural)</param>
        /// <returns>Lista de clientes del tipo especificado</returns>
        Task<List<Cliente>> ObtenerPorTipo(string tipoCliente);

        /// <summary>
        /// Verifica si existe un cliente con el NIT especificado.
        /// </summary>
        /// <param name="nit">Número de identificación tributaria</param>
        /// <returns>True si existe, false en caso contrario</returns>
        Task<bool> ExisteNit(string nit);

        /// <summary>
        /// Verifica si existe un cliente con el correo electrónico especificado.
        /// </summary>
        /// <param name="correo">Correo electrónico</param>
        /// <returns>True si existe, false en caso contrario</returns>
        Task<bool> ExisteCorreo(string correo);

        /// <summary>
        /// Obtiene clientes por país.
        /// </summary>
        /// <param name="pais">Nombre del país</param>
        /// <returns>Lista de clientes del país</returns>
        Task<List<Cliente>> ObtenerPorPais(string pais);

        /// <summary>
        /// Busca clientes por razón social (búsqueda parcial).
        /// </summary>
        /// <param name="razonSocial">Razón social a buscar</param>
        /// <returns>Lista de clientes que coinciden con la búsqueda</returns>
        Task<List<Cliente>> BuscarPorRazonSocial(string razonSocial);

        /// <summary>
        /// Obtiene clientes registrados en un rango de fechas.
        /// </summary>
        /// <param name="fechaInicio">Fecha de inicio</param>
        /// <param name="fechaFin">Fecha de fin</param>
        /// <returns>Lista de clientes registrados en el rango</returns>
        Task<List<Cliente>> ObtenerPorRangoFechas(DateTime fechaInicio, DateTime fechaFin);
    }
}
