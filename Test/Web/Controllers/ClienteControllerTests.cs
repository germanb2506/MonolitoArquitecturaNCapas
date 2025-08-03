using App.Dto;
using App.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Test.Helpers;
using Web.Controllers;

namespace Test.Web.Controllers
{
    public class ClienteControllerTests
    {
        private readonly Mock<IClienteService> _mockClienteService;
        private readonly ClienteController _clienteController;

        public ClienteControllerTests()
        {
            _mockClienteService = new Mock<IClienteService>();
            _clienteController = new ClienteController(_mockClienteService.Object);
        }

        [Fact]
        public async Task Index_DeberiaRetornarViewConClientes_CuandoExistenClientes()
        {
            // Arrange
            var clientesDto = TestDataHelper.CreateSampleClienteDtos(3);
            var result = Result<List<ClienteDto>>.Success(clientesDto, "Clientes obtenidos exitosamente");

            _mockClienteService
                .Setup(x => x.ObtenerClientes())
                .ReturnsAsync(result);

            // Act
            var actionResult = await _clienteController.Index();

            // Assert
            var viewResult = actionResult.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeOfType<List<ClienteDto>>().Subject;
            model.Should().HaveCount(3);
        }

        [Fact]
        public async Task Index_DeberiaRetornarViewVacio_CuandoNoExistenClientes()
        {
            // Arrange
            var result = Result<List<ClienteDto>>.Success(new List<ClienteDto>(), "No hay clientes");

            _mockClienteService
                .Setup(x => x.ObtenerClientes())
                .ReturnsAsync(result);

            // Act
            var actionResult = await _clienteController.Index();

            // Assert
            var viewResult = actionResult.Should().BeOfType<ViewResult>().Subject;
            var model = viewResult.Model.Should().BeOfType<List<ClienteDto>>().Subject;
            model.Should().BeEmpty();
        }

        [Fact]
        public async Task Crear_DeberiaRetornarJsonExitoso_CuandoClienteSeCreaCorrectamente()
        {
            // Arrange
            var clienteDto = TestDataHelper.CreateSampleClienteDto();
            var result = Result<ClienteDto>.Success(clienteDto, "Cliente creado exitosamente");

            _mockClienteService
                .Setup(x => x.CrearCliente(clienteDto))
                .ReturnsAsync(result);

            // Act
            var actionResult = await _clienteController.Crear(clienteDto);

            // Assert
            var jsonResult = actionResult.Should().BeOfType<JsonResult>().Subject;
            var resultData = jsonResult.Value.Should().BeOfType<Result<ClienteDto>>().Subject;
            resultData.IsExitoso.Should().BeTrue();
            resultData.Data.Should().Be(clienteDto);
        }

        [Fact]
        public async Task Crear_DeberiaRetornarJsonConError_CuandoClienteNoSePuedeCrear()
        {
            // Arrange
            var clienteDto = TestDataHelper.CreateSampleClienteDto();
            var result = Result<ClienteDto>.Error(ResponseCode.BadRequest, "Error al crear cliente");

            _mockClienteService
                .Setup(x => x.CrearCliente(clienteDto))
                .ReturnsAsync(result);

            // Act
            var actionResult = await _clienteController.Crear(clienteDto);

            // Assert
            var jsonResult = actionResult.Should().BeOfType<JsonResult>().Subject;
            var resultData = jsonResult.Value.Should().BeOfType<Result<ClienteDto>>().Subject;
            resultData.IsExitoso.Should().BeFalse();
            resultData.Message.Should().Be("Error al crear cliente");
        }

        [Fact]
        public async Task Crear_DeberiaRetornarMensajeDeError_CuandoClienteEsNull()
        {
            // Arrange
            ClienteDto? clienteDto = null;

            // Act
            var actionResult = await _clienteController.Crear(clienteDto);

            // Assert
            var jsonResult = actionResult.Should().BeOfType<JsonResult>().Subject;
            jsonResult.Value.Should().Be("Aca no llegaron datos");
        }
    }
} 