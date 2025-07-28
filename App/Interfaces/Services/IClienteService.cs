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
    }
}
