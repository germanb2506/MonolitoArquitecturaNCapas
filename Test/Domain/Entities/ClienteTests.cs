using Domain.Entities;
using Test.Helpers;

namespace Test.Domain.Entities
{
    public class ClienteTests
    {
        [Fact]
        public void Cliente_DeberiaTenerPropiedadesCorrectas()
        {
            // Arrange
            var cliente = TestDataHelper.CreateSampleCliente();

            // Assert
            cliente.Id.Should().Be(1);
            cliente.RazonSocial.Should().Be("Empresa Test 1");
            cliente.Nit.Should().Be("12345678-1");
            cliente.TipoCliente.Should().Be("Jurídica");
            cliente.RepresentanteLegal.Should().Be("Juan Pérez 1");
            cliente.CorreoContacto.Should().Be("contacto1@empresa.com");
            cliente.TelefonoContacto.Should().Be("3001234561");
            cliente.Direccion.Should().Be("Calle 1 # 1-1");
            cliente.Ciudad.Should().Be("Bogotá");
            cliente.Pais.Should().Be("Colombia");
            cliente.PaginaWeb.Should().Be("www.empresa1.com");
            cliente.Notas.Should().Be("Notas de prueba para cliente 1");
            cliente.Activo.Should().BeTrue();
            cliente.FechaRegistro.Should().BeCloseTo(DateTime.Now.AddDays(-1), TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void Cliente_DeberiaPermitirPropiedadesOpcionalesNulas()
        {
            // Arrange & Act
            var cliente = new Cliente
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
            cliente.PaginaWeb.Should().BeNull();
            cliente.Notas.Should().BeNull();
        }

        [Fact]
        public void Cliente_DeberiaTenerValoresPorDefectoCorrectos()
        {
            // Arrange & Act
            var cliente = new Cliente();

            // Assert
            cliente.RazonSocial.Should().BeNull();
            cliente.Nit.Should().BeNull();
            cliente.TipoCliente.Should().BeNull();
            cliente.RepresentanteLegal.Should().BeNull();
            cliente.CorreoContacto.Should().BeNull();
            cliente.TelefonoContacto.Should().BeNull();
            cliente.Direccion.Should().BeNull();
            cliente.Ciudad.Should().BeNull();
            cliente.Pais.Should().BeNull();
            cliente.Activo.Should().BeFalse();
            cliente.FechaRegistro.Should().Be(default(DateTime));
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(999999)]
        public void Cliente_DeberiaAceptarDiferentesIds(int id)
        {
            // Arrange & Act
            var cliente = TestDataHelper.CreateSampleCliente(id);

            // Assert
            cliente.Id.Should().Be(id);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Cliente_DeberiaPermitirDiferentesEstadosActivo(bool activo)
        {
            // Arrange & Act
            var cliente = TestDataHelper.CreateSampleCliente();
            cliente.Activo = activo;

            // Assert
            cliente.Activo.Should().Be(activo);
        }
    }
} 