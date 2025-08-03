using App.Dto;
using Test.Helpers;

namespace Test.App.Dto
{
    public class ClienteDtoTests
    {
        [Fact]
        public void ClienteDto_DeberiaTenerPropiedadesCorrectas()
        {
            // Arrange
            var clienteDto = TestDataHelper.CreateSampleClienteDto();

            // Assert
            clienteDto.Id.Should().Be(1);
            clienteDto.RazonSocial.Should().Be("Empresa Test 1");
            clienteDto.Nit.Should().Be("12345678-1");
            clienteDto.TipoCliente.Should().Be("Jurídica");
            clienteDto.RepresentanteLegal.Should().Be("Juan Pérez 1");
            clienteDto.CorreoContacto.Should().Be("contacto1@empresa.com");
            clienteDto.TelefonoContacto.Should().Be("3001234561");
            clienteDto.Direccion.Should().Be("Calle 1 # 1-1");
            clienteDto.Ciudad.Should().Be("Bogotá");
            clienteDto.Pais.Should().Be("Colombia");
            clienteDto.PaginaWeb.Should().Be("www.empresa1.com");
            clienteDto.Notas.Should().Be("Notas de prueba para cliente 1");
            clienteDto.Activo.Should().BeTrue();
            clienteDto.FechaRegistro.Should().BeCloseTo(DateTime.Now.AddDays(-1), TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void ClienteDto_DeberiaPermitirPropiedadesOpcionalesNulas()
        {
            // Arrange & Act
            var clienteDto = new ClienteDto
            {
                Id = 1,
                RazonSocial = "Empresa Test",
                Nit = "12345678-1",
                TipoCliente = "Jurídica",
                RepresentanteLegal = "Juan Pérez",
                CorreoContacto = "contacto@empresa.com",
                TelefonoContacto = "3001234561",
                Direccion = "Calle 1 # 1-1",
                Ciudad = "Bogotá",
                Pais = "Colombia",
                PaginaWeb = null,
                Notas = null,
                Activo = true,
                FechaRegistro = DateTime.Now
            };

            // Assert
            clienteDto.PaginaWeb.Should().BeNull();
            clienteDto.Notas.Should().BeNull();
        }

        [Fact]
        public void ClienteDto_DeberiaTenerValoresPorDefectoCorrectos()
        {
            // Arrange & Act
            var clienteDto = new ClienteDto();

            // Assert
            clienteDto.RazonSocial.Should().BeNull();
            clienteDto.Nit.Should().BeNull();
            clienteDto.TipoCliente.Should().BeNull();
            clienteDto.RepresentanteLegal.Should().BeNull();
            clienteDto.CorreoContacto.Should().BeNull();
            clienteDto.TelefonoContacto.Should().BeNull();
            clienteDto.Direccion.Should().BeNull();
            clienteDto.Ciudad.Should().BeNull();
            clienteDto.Pais.Should().BeNull();
            clienteDto.Activo.Should().BeFalse();
            clienteDto.FechaRegistro.Should().Be(default(DateTime));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(999999)]
        public void ClienteDto_DeberiaAceptarDiferentesIds(int id)
        {
            // Arrange & Act
            var clienteDto = TestDataHelper.CreateSampleClienteDto(id);

            // Assert
            clienteDto.Id.Should().Be(id);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void ClienteDto_DeberiaPermitirDiferentesEstadosActivo(bool activo)
        {
            // Arrange & Act
            var clienteDto = TestDataHelper.CreateSampleClienteDto();
            clienteDto.Activo = activo;

            // Assert
            clienteDto.Activo.Should().Be(activo);
        }

        [Fact]
        public void ClienteDto_DeberiaSerIgualAOtroConMismosValores()
        {
            // Arrange
            var clienteDto1 = TestDataHelper.CreateSampleClienteDto();
            var clienteDto2 = TestDataHelper.CreateSampleClienteDto();

            // Act & Assert
            clienteDto1.Id.Should().Be(clienteDto2.Id);
            clienteDto1.RazonSocial.Should().Be(clienteDto2.RazonSocial);
            clienteDto1.Nit.Should().Be(clienteDto2.Nit);
        }
    }
} 