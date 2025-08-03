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
                // Validar que el NIT no exista
                if (await _clienteRepo.ExisteNit(clienteDto.Nit))
                {
                    return Result<ClienteDto>.Error(ResponseCode.BadRequest, "Ya existe un cliente con este NIT");
                }

                // Validar que el correo no exista
                if (await _clienteRepo.ExisteCorreo(clienteDto.CorreoContacto))
                {
                    return Result<ClienteDto>.Error(ResponseCode.BadRequest, "Ya existe un cliente con este correo electrónico");
                }

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
                var clienteExistente = await _clienteRepo.Obtener(c => c.Id == clienteDto.Id);
                if (clienteExistente == null)
                {
                    return Result<ClienteDto>.Error(ResponseCode.NotFound, "Cliente no encontrado");
                }

                // Validar que el NIT no exista en otro cliente
                var clienteConMismoNit = await _clienteRepo.ObtenerPorNit(clienteDto.Nit);
                if (clienteConMismoNit != null && clienteConMismoNit.Id != clienteDto.Id)
                {
                    return Result<ClienteDto>.Error(ResponseCode.BadRequest, "Ya existe otro cliente con este NIT");
                }

                // Validar que el correo no exista en otro cliente
                var clienteConMismoCorreo = await _clienteRepo.ObtenerPorCorreo(clienteDto.CorreoContacto);
                if (clienteConMismoCorreo != null && clienteConMismoCorreo.Id != clienteDto.Id)
                {
                    return Result<ClienteDto>.Error(ResponseCode.BadRequest, "Ya existe otro cliente con este correo electrónico");
                }

                _mapper.Map(clienteDto, clienteExistente);
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

        // Nuevos métodos usando los métodos específicos del repositorio
        public async Task<Result<ClienteDto>> ObtenerClientePorNit(string nit)
        {
            try
            {
                var cliente = await _clienteRepo.ObtenerPorNit(nit);
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

        public async Task<Result<ClienteDto>> ObtenerClientePorCorreo(string correo)
        {
            try
            {
                var cliente = await _clienteRepo.ObtenerPorCorreo(correo);
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

        public async Task<Result<List<ClienteDto>>> ObtenerClientesActivos()
        {
            try
            {
                var clientes = await _clienteRepo.ObtenerActivos();
                var clientesDto = _mapper.Map<List<ClienteDto>>(clientes);
                return Result<List<ClienteDto>>.Success(clientesDto, "Clientes activos obtenidos exitosamente");
            }
            catch (Exception ex)
            {
                return Result<List<ClienteDto>>.Error(ResponseCode.InternalServerError, "Error al obtener los clientes activos", new List<string> { ex.Message });
            }
        }

        public async Task<Result<List<ClienteDto>>> ObtenerClientesPorCiudad(string ciudad)
        {
            try
            {
                var clientes = await _clienteRepo.ObtenerPorCiudad(ciudad);
                var clientesDto = _mapper.Map<List<ClienteDto>>(clientes);
                return Result<List<ClienteDto>>.Success(clientesDto, $"Clientes de {ciudad} obtenidos exitosamente");
            }
            catch (Exception ex)
            {
                return Result<List<ClienteDto>>.Error(ResponseCode.InternalServerError, "Error al obtener los clientes por ciudad", new List<string> { ex.Message });
            }
        }

        public async Task<Result<List<ClienteDto>>> ObtenerClientesPorPais(string pais)
        {
            try
            {
                var clientes = await _clienteRepo.ObtenerPorPais(pais);
                var clientesDto = _mapper.Map<List<ClienteDto>>(clientes);
                return Result<List<ClienteDto>>.Success(clientesDto, $"Clientes de {pais} obtenidos exitosamente");
            }
            catch (Exception ex)
            {
                return Result<List<ClienteDto>>.Error(ResponseCode.InternalServerError, "Error al obtener los clientes por país", new List<string> { ex.Message });
            }
        }

        public async Task<Result<List<ClienteDto>>> ObtenerClientesPorTipo(string tipoCliente)
        {
            try
            {
                var clientes = await _clienteRepo.ObtenerPorTipo(tipoCliente);
                var clientesDto = _mapper.Map<List<ClienteDto>>(clientes);
                return Result<List<ClienteDto>>.Success(clientesDto, $"Clientes de tipo {tipoCliente} obtenidos exitosamente");
            }
            catch (Exception ex)
            {
                return Result<List<ClienteDto>>.Error(ResponseCode.InternalServerError, "Error al obtener los clientes por tipo", new List<string> { ex.Message });
            }
        }

        public async Task<Result<List<ClienteDto>>> BuscarClientesPorRazonSocial(string razonSocial)
        {
            try
            {
                var clientes = await _clienteRepo.BuscarPorRazonSocial(razonSocial);
                var clientesDto = _mapper.Map<List<ClienteDto>>(clientes);
                return Result<List<ClienteDto>>.Success(clientesDto, "Búsqueda de clientes completada exitosamente");
            }
            catch (Exception ex)
            {
                return Result<List<ClienteDto>>.Error(ResponseCode.InternalServerError, "Error al buscar clientes", new List<string> { ex.Message });
            }
        }

        public async Task<Result<List<ClienteDto>>> ObtenerClientesPorRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            try
            {
                var clientes = await _clienteRepo.ObtenerPorRangoFechas(fechaInicio, fechaFin);
                var clientesDto = _mapper.Map<List<ClienteDto>>(clientes);
                return Result<List<ClienteDto>>.Success(clientesDto, $"Clientes registrados entre {fechaInicio:dd/MM/yyyy} y {fechaFin:dd/MM/yyyy} obtenidos exitosamente");
            }
            catch (Exception ex)
            {
                return Result<List<ClienteDto>>.Error(ResponseCode.InternalServerError, "Error al obtener los clientes por rango de fechas", new List<string> { ex.Message });
            }
        }

        public async Task<Result<bool>> ValidarNitUnico(string nit, int? idExcluir = null)
        {
            try
            {
                var cliente = await _clienteRepo.ObtenerPorNit(nit);
                if (cliente == null)
                {
                    return Result<bool>.Success(true, "NIT disponible");
                }

                if (idExcluir.HasValue && cliente.Id == idExcluir.Value)
                {
                    return Result<bool>.Success(true, "NIT disponible");
                }

                return Result<bool>.Success(false, "NIT ya existe");
            }
            catch (Exception ex)
            {
                return Result<bool>.Error(ResponseCode.InternalServerError, "Error al validar NIT", new List<string> { ex.Message });
            }
        }

        public async Task<Result<bool>> ValidarCorreoUnico(string correo, int? idExcluir = null)
        {
            try
            {
                var cliente = await _clienteRepo.ObtenerPorCorreo(correo);
                if (cliente == null)
                {
                    return Result<bool>.Success(true, "Correo disponible");
                }

                if (idExcluir.HasValue && cliente.Id == idExcluir.Value)
                {
                    return Result<bool>.Success(true, "Correo disponible");
                }

                return Result<bool>.Success(false, "Correo ya existe");
            }
            catch (Exception ex)
            {
                return Result<bool>.Error(ResponseCode.InternalServerError, "Error al validar correo", new List<string> { ex.Message });
            }
        }
    }
}
