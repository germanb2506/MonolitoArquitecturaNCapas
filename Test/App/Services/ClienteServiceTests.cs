using App.Dto;
using App.Interfaces.Repos;
using App.Services;
using AutoMapper;
using Domain.Entities;
using System.Linq.Expressions;
using Test.Helpers;

namespace Test.App.Services
{
    public class ClienteServiceTests
    {
        private readonly Mock<IClienteRepo> _mockClienteRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ClienteService _clienteService;

        public ClienteServiceTests()
        {
            _mockClienteRepo = new Mock<IClienteRepo>();
            _mockMapper = new Mock<IMapper>();
            _clienteService = new ClienteService(_mockClienteRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task CrearCliente_DeberiaRetornarExito_CuandoClienteSeCreaCorrectamente()
        {
            // Arrange
            var clienteDto = TestDataHelper.CreateSampleClienteDto();
            var cliente = TestDataHelper.CreateSampleCliente();

            _mockMapper.Setup(x => x.Map<Cliente>(clienteDto)).Returns(cliente);
            _mockClienteRepo.Setup(x => x.Crear(cliente)).Returns(Task.CompletedTask);

            // Act
            var result = await _clienteService.CrearCliente(clienteDto);

            // Assert
            result.IsExitoso.Should().BeTrue();
            result.Data.Should().Be(clienteDto);
            result.Message.Should().Be("Cliente creado exitosamente");
        }

        [Fact]
        public async Task CrearCliente_DeberiaRetornarError_CuandoRepositorioFalla()
        {
            // Arrange
            var clienteDto = TestDataHelper.CreateSampleClienteDto();
            var cliente = TestDataHelper.CreateSampleCliente();

            _mockMapper.Setup(x => x.Map<Cliente>(clienteDto)).Returns(cliente);
            _mockClienteRepo.Setup(x => x.Crear(cliente)).ThrowsAsync(new Exception("Error de base de datos"));

            // Act
            var result = await _clienteService.CrearCliente(clienteDto);

            // Assert
            result.IsExitoso.Should().BeFalse();
            result.Message.Should().Be("Error al crear el cliente");
            result.ResponseCode.Should().Be(ResponseCode.InternalServerError);
        }

        [Fact]
        public async Task ObtenerClientes_DeberiaRetornarListaDeClientes_CuandoExistenClientes()
        {
            // Arrange
            var clientes = TestDataHelper.CreateSampleClientes(3);
            var clientesDto = TestDataHelper.CreateSampleClienteDtos(3);

            _mockClienteRepo.Setup(x => x.ObtenerTodos(null)).ReturnsAsync(clientes);
            _mockMapper.Setup(x => x.Map<List<ClienteDto>>(clientes)).Returns(clientesDto);

            // Act
            var result = await _clienteService.ObtenerClientes();

            // Assert
            result.IsExitoso.Should().BeTrue();
            result.Data.Should().HaveCount(3);
            result.Message.Should().Be("Clientes obtenidos exitosamente");
        }

        [Fact]
        public async Task ObtenerClientes_DeberiaRetornarError_CuandoRepositorioFalla()
        {
            // Arrange
            _mockClienteRepo.Setup(x => x.ObtenerTodos(null)).ThrowsAsync(new Exception("Error de base de datos"));

            // Act
            var result = await _clienteService.ObtenerClientes();

            // Assert
            result.IsExitoso.Should().BeFalse();
            result.Message.Should().Be("Error al obtener los clientes");
            result.ResponseCode.Should().Be(ResponseCode.InternalServerError);
        }

        [Fact]
        public async Task ObtenerClientePorId_DeberiaRetornarCliente_CuandoClienteExiste()
        {
            // Arrange
            var id = 1;
            var cliente = TestDataHelper.CreateSampleCliente(id);
            var clienteDto = TestDataHelper.CreateSampleClienteDto(id);

            _mockClienteRepo.Setup(x => x.Obtener(It.IsAny<Expression<Func<Cliente, bool>>>(), true)).ReturnsAsync(cliente);
            _mockMapper.Setup(x => x.Map<ClienteDto>(cliente)).Returns(clienteDto);

            // Act
            var result = await _clienteService.ObtenerClientePorId(id);

            // Assert
            result.IsExitoso.Should().BeTrue();
            result.Data.Should().Be(clienteDto);
            result.Message.Should().Be("Cliente obtenido exitosamente");
        }

        [Fact]
        public async Task ObtenerClientePorId_DeberiaRetornarError_CuandoClienteNoExiste()
        {
            // Arrange
            var id = 999;

            _mockClienteRepo.Setup(x => x.Obtener(It.IsAny<Expression<Func<Cliente, bool>>>(), true)).ReturnsAsync((Cliente?)null);

            // Act
            var result = await _clienteService.ObtenerClientePorId(id);

            // Assert
            result.IsExitoso.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Message.Should().Be("Cliente no encontrado");
            result.ResponseCode.Should().Be(ResponseCode.NotFound);
        }

        [Fact]
        public async Task ActualizarCliente_DeberiaRetornarExito_CuandoClienteSeActualizaCorrectamente()
        {
            // Arrange
            var clienteDto = TestDataHelper.CreateSampleClienteDto();
            var cliente = TestDataHelper.CreateSampleCliente();
            var clienteExistente = TestDataHelper.CreateSampleCliente();

            _mockMapper.Setup(x => x.Map<Cliente>(clienteDto)).Returns(cliente);
            _mockClienteRepo.Setup(x => x.Obtener(It.IsAny<Expression<Func<Cliente, bool>>>(), true)).ReturnsAsync(clienteExistente);
            _mockMapper.Setup(x => x.Map(clienteDto, clienteExistente)).Verifiable();
            _mockClienteRepo.Setup(x => x.Grabar()).Returns(Task.CompletedTask);

            // Act
            var result = await _clienteService.ActualizarCliente(clienteDto);

            // Assert
            result.IsExitoso.Should().BeTrue();
            result.Data.Should().Be(clienteDto);
            result.Message.Should().Be("Cliente actualizado exitosamente");
        }

        [Fact]
        public async Task ActualizarCliente_DeberiaRetornarError_CuandoClienteNoExiste()
        {
            // Arrange
            var clienteDto = TestDataHelper.CreateSampleClienteDto();
            var cliente = TestDataHelper.CreateSampleCliente();

            _mockMapper.Setup(x => x.Map<Cliente>(clienteDto)).Returns(cliente);
            _mockClienteRepo.Setup(x => x.Obtener(It.IsAny<Expression<Func<Cliente, bool>>>(), true)).ReturnsAsync((Cliente?)null);

            // Act
            var result = await _clienteService.ActualizarCliente(clienteDto);

            // Assert
            result.IsExitoso.Should().BeFalse();
            result.Data.Should().BeNull();
            result.Message.Should().Be("Cliente no encontrado");
            result.ResponseCode.Should().Be(ResponseCode.NotFound);
        }

        [Fact]
        public async Task EliminarCliente_DeberiaRetornarExito_CuandoClienteSeEliminaCorrectamente()
        {
            // Arrange
            var id = 1;
            var cliente = TestDataHelper.CreateSampleCliente(id);

            _mockClienteRepo.Setup(x => x.Obtener(It.IsAny<Expression<Func<Cliente, bool>>>(), true)).ReturnsAsync(cliente);
            _mockClienteRepo.Setup(x => x.Remover(cliente)).Returns(Task.CompletedTask);

            // Act
            var result = await _clienteService.EliminarCliente(id);

            // Assert
            result.IsExitoso.Should().BeTrue();
            result.Data.Should().BeTrue();
            result.Message.Should().Be("Cliente eliminado exitosamente");
        }

        [Fact]
        public async Task EliminarCliente_DeberiaRetornarError_CuandoClienteNoExiste()
        {
            // Arrange
            var id = 999;

            _mockClienteRepo.Setup(x => x.Obtener(It.IsAny<Expression<Func<Cliente, bool>>>(), It.IsAny<bool>())).ReturnsAsync((Cliente?)null);

            // Act
            var result = await _clienteService.EliminarCliente(id);

            // Assert
            result.IsExitoso.Should().BeFalse();
            result.Data.Should().BeFalse();
            result.Message.Should().Be("Cliente no encontrado");
            result.ResponseCode.Should().Be(ResponseCode.NotFound);
        }

        [Fact]
        public async Task GetProyCliente_DeberiaRetornarClienteProyectado_CuandoClienteExiste()
        {
            // Arrange
            var id = 1;
            var clienteDto = new ClienteDto
            {
                Id = id,
                RazonSocial = "Empresa Test 1",
                CorreoContacto = "contacto1@empresa.com"
            };

            _mockClienteRepo.Setup(x => x.GetProy(It.IsAny<Expression<Func<Cliente, bool>>>(), It.IsAny<Expression<Func<Cliente, ClienteDto>>>()))
                .ReturnsAsync(clienteDto);

            // Act
            var result = await _clienteService.GetProyCliente(id);

            // Assert
            result.IsExitoso.Should().BeTrue();
            result.Data.Should().Be(clienteDto);
            result.Message.Should().Be("Cliente obtenido exitosamente");
        }

        [Fact]
        public async Task GetAllProyClientes_DeberiaRetornarListaDeClientesProyectados()
        {
            // Arrange
            var clientesDto = new List<ClienteDto>
            {
                new ClienteDto { Id = 1, RazonSocial = "Empresa 1", CorreoContacto = "contacto1@empresa.com" },
                new ClienteDto { Id = 2, RazonSocial = "Empresa 2", CorreoContacto = "contacto2@empresa.com" }
            };

            _mockClienteRepo.Setup(x => x.GetAllProy(It.IsAny<Expression<Func<Cliente, bool>>>(), It.IsAny<Expression<Func<Cliente, ClienteDto>>>()))
                .ReturnsAsync(clientesDto);

            // Act
            var result = await _clienteService.GetAllProyClientes();

            // Assert
            result.IsExitoso.Should().BeTrue();
            result.Data.Should().HaveCount(2);
            result.Message.Should().Be("Clientes obtenidos exitosamente");
        }
    }
} 