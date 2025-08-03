using App.Dto;

namespace App.Interfaces.Services
{
    public interface IClienteService
    {
        /// <summary>
        /// Crea un nuevo cliente.
        /// </summary>
        Task<Result<ClienteDto>> CrearCliente(ClienteDto clienteDto);

        /// <summary>
        /// Obtiene todos los clientes.
        /// </summary>
        Task<Result<List<ClienteDto>>> ObtenerClientes();

        /// <summary>
        /// Obtiene un cliente por su ID.
        /// </summary>
        Task<Result<ClienteDto>> ObtenerClientePorId(int id);

        /// <summary>
        /// Actualiza un cliente existente.
        /// </summary>
        Task<Result<ClienteDto>> ActualizarCliente(ClienteDto clienteDto);

        /// <summary>
        /// Elimina un cliente por su ID.
        /// </summary>
        Task<Result<bool>> EliminarCliente(int id);

        /// <summary>
        /// Obtiene un cliente proyectado como ClienteDto por su ID.
        /// </summary>
        Task<Result<ClienteDto>> GetProyCliente(int id);

        /// <summary>
        /// Obtiene todos los clientes proyectados como ClienteDto.
        /// </summary>
        Task<Result<List<ClienteDto>>> GetAllProyClientes();

        /// <summary>
        /// Obtiene un cliente por su NIT.
        /// </summary>
        Task<Result<ClienteDto>> ObtenerClientePorNit(string nit);

        /// <summary>
        /// Obtiene un cliente por su correo electrónico.
        /// </summary>
        Task<Result<ClienteDto>> ObtenerClientePorCorreo(string correo);

        /// <summary>
        /// Obtiene todos los clientes activos.
        /// </summary>
        Task<Result<List<ClienteDto>>> ObtenerClientesActivos();

        /// <summary>
        /// Obtiene todos los clientes de una ciudad específica.
        /// </summary>
        Task<Result<List<ClienteDto>>> ObtenerClientesPorCiudad(string ciudad);

        /// <summary>
        /// Obtiene todos los clientes de un país específico.
        /// </summary>
        Task<Result<List<ClienteDto>>> ObtenerClientesPorPais(string pais);

        /// <summary>
        /// Obtiene todos los clientes de un tipo específico.
        /// </summary>
        Task<Result<List<ClienteDto>>> ObtenerClientesPorTipo(string tipoCliente);

        /// <summary>
        /// Busca clientes por razón social (búsqueda parcial).
        /// </summary>
        Task<Result<List<ClienteDto>>> BuscarClientesPorRazonSocial(string razonSocial);

        /// <summary>
        /// Obtiene clientes registrados en un rango de fechas.
        /// </summary>
        Task<Result<List<ClienteDto>>> ObtenerClientesPorRangoFechas(DateTime fechaInicio, DateTime fechaFin);

        /// <summary>
        /// Valida si un NIT es único, excluyendo opcionalmente un cliente específico.
        /// </summary>
        Task<Result<bool>> ValidarNitUnico(string nit, int? idExcluir = null);

        /// <summary>
        /// Valida si un correo electrónico es único, excluyendo opcionalmente un cliente específico.
        /// </summary>
        Task<Result<bool>> ValidarCorreoUnico(string correo, int? idExcluir = null);
    }
}
