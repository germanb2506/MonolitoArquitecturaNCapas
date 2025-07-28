using App.Dto;
using App.Interfaces.Repos;
using App.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using System.Linq.Expressions;

namespace App.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepo _clienteRepo;
        private readonly IMapper _mapper;

        public ClienteService(IClienteRepo clienteRepo, IMapper mapper)
        {
            _clienteRepo = clienteRepo;
            _mapper = mapper;
        }

        public async Task<Result<ClienteDto>> CrearCliente(ClienteDto clienteDto)
        {
            try
            {
                var cliente = _mapper.Map<Cliente>(clienteDto);
                await _clienteRepo.Crear(cliente);
                return Result<ClienteDto>.Success(clienteDto, "Cliente creado exitosamente");
            }
            catch (Exception ex)
            {
                return Result<ClienteDto>.Error(ResponseCode.InternalServerError, "Error al crear el cliente", new List<string> { ex.Message });
            }
        }

        public async Task<Result<List<ClienteDto>>> ObtenerClientes()
        {
            try
            {
                
                var clientes = await _clienteRepo.ObtenerTodos();
                var clientesDto = _mapper.Map<List<ClienteDto>>(clientes);
                return Result<List<ClienteDto>>.Success(clientesDto, "Clientes obtenidos exitosamente");
            }
            catch (Exception ex)
            {
                return Result<List<ClienteDto>>.Error(ResponseCode.InternalServerError, "Error al obtener los clientes", new List<string> { ex.Message });
            }
        }

        public async Task<Result<ClienteDto>> ObtenerClientePorId(int id)
        {
            try
            {
                var cliente = await _clienteRepo.Obtener(c => c.Id == id);
                if (cliente == null)
                {
                    return Result<ClienteDto>.Error(ResponseCode.NotFound, "Cliente no encontrado");
                }

                var clienteDto = _mapper.Map<ClienteDto>(cliente);
                return Result<ClienteDto>.Success(clienteDto, "Cliente obtenido exitosamente");
            }
            catch (Exception ex)
            {
                return Result<ClienteDto>.Error(ResponseCode.InternalServerError, "Error al obtener el cliente", new List<string> { ex.Message });
            }
        }

        public async Task<Result<ClienteDto>> ActualizarCliente(ClienteDto clienteDto)
        {
            try
            {
                var cliente = _mapper.Map<Cliente>(clienteDto);
                var existente = await _clienteRepo.Obtener(c => c.Id == cliente.Id);

                if (existente == null)
                {
                    return Result<ClienteDto>.Error(ResponseCode.NotFound, "Cliente no encontrado");
                }

                _mapper.Map(clienteDto, existente);
                await _clienteRepo.Grabar();

                return Result<ClienteDto>.Success(clienteDto, "Cliente actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return Result<ClienteDto>.Error(ResponseCode.InternalServerError, "Error al actualizar el cliente", new List<string> { ex.Message });
            }
        }

        public async Task<Result<bool>> EliminarCliente(int id)
        {
            try
            {
                var cliente = await _clienteRepo.Obtener(c => c.Id == id);
                if (cliente == null)
                {
                    return Result<bool>.Error(ResponseCode.NotFound, "Cliente no encontrado");
                }

                await _clienteRepo.Remover(cliente);
                return Result<bool>.Success(true, "Cliente eliminado exitosamente");
            }
            catch (Exception ex)
            {
                return Result<bool>.Error(ResponseCode.InternalServerError, "Error al eliminar el cliente", new List<string> { ex.Message });
            }
        }

        public async Task<Result<ClienteDto>> GetProyCliente(int id)
        {
            try
            {
                Expression<Func<Cliente, ClienteDto>> selector = c => new ClienteDto
                {
                    Id = c.Id,
                    RazonSocial = c.RazonSocial,
                    CorreoContacto = c.CorreoContacto,
                };

                var clienteDto = await _clienteRepo.GetProy(c => c.Id == id, selector);

                if (clienteDto == null)
                {
                    return Result<ClienteDto>.Error(ResponseCode.NotFound, "Cliente no encontrado");
                }

                return Result<ClienteDto>.Success(clienteDto, "Cliente obtenido exitosamente");
            }
            catch (Exception ex)
            {
                return Result<ClienteDto>.Error(ResponseCode.InternalServerError, "Error al obtener el cliente", new List<string> { ex.Message });
            }
        }

        public async Task<Result<List<ClienteDto>>> GetAllProyClientes()
        {
            try
            {
                Expression<Func<Cliente, ClienteDto>> selector = c => new ClienteDto
                {
                    Id = c.Id,
                    RazonSocial = c.RazonSocial,
                    CorreoContacto = c.CorreoContacto,
                };

                var clientesDto = await _clienteRepo.GetAllProy(null, selector);
                return Result<List<ClienteDto>>.Success(clientesDto, "Clientes obtenidos exitosamente");
            }
            catch (Exception ex)
            {
                return Result<List<ClienteDto>>.Error(ResponseCode.InternalServerError, "Error al obtener los clientes", new List<string> { ex.Message });
            }
        }
    }
}
