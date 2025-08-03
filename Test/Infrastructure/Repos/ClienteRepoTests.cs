using Domain.Entities;
using FluentAssertions;
using Infrastructure;
using Infrastructure.Repos;
using Microsoft.EntityFrameworkCore;
using Test.Helpers;

namespace Test.Infrastructure.Repos
{
    public class ClienteRepoTests : IDisposable
    {
        private readonly DbContexto _context;
        private readonly ClienteRepo _clienteRepo;

        public ClienteRepoTests()
        {
            var options = new DbContextOptionsBuilder<DbContexto>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DbContexto(options);
            _clienteRepo = new ClienteRepo(_context);
        }

        [Fact]
        public async Task ObtenerPorNit_DeberiaRetornarCliente_CuandoExiste()
        {
            // Arrange
            var cliente = TestDataHelper.CreateSampleCliente(1);
            await _context.Set<Cliente>().AddAsync(cliente);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _clienteRepo.ObtenerPorNit(cliente.Nit);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Nit.Should().Be(cliente.Nit);
        }

        [Fact]
        public async Task ObtenerPorNit_DeberiaRetornarNull_CuandoNoExiste()
        {
            // Act
            var resultado = await _clienteRepo.ObtenerPorNit("999999999");

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task ObtenerPorCorreo_DeberiaRetornarCliente_CuandoExiste()
        {
            // Arrange
            var cliente = TestDataHelper.CreateSampleCliente(1);
            await _context.Set<Cliente>().AddAsync(cliente);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _clienteRepo.ObtenerPorCorreo(cliente.CorreoContacto);

            // Assert
            resultado.Should().NotBeNull();
            resultado.CorreoContacto.Should().Be(cliente.CorreoContacto);
        }

        [Fact]
        public async Task ObtenerPorCorreo_DeberiaRetornarNull_CuandoNoExiste()
        {
            // Act
            var resultado = await _clienteRepo.ObtenerPorCorreo("noexiste@test.com");

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task ObtenerPorCiudad_DeberiaRetornarClientes_CuandoExisten()
        {
            // Arrange
            var cliente1 = TestDataHelper.CreateSampleCliente(1);
            var cliente2 = TestDataHelper.CreateSampleCliente(2);
            cliente1.Ciudad = "Bogotá";
            cliente2.Ciudad = "Bogotá";
            
            await _context.Set<Cliente>().AddRangeAsync(cliente1, cliente2);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _clienteRepo.ObtenerPorCiudad("Bogotá");

            // Assert
            resultado.Should().HaveCount(2);
            resultado.Should().OnlyContain(c => c.Ciudad == "Bogotá");
        }

        [Fact]
        public async Task ObtenerActivos_DeberiaRetornarSoloClientesActivos()
        {
            // Arrange
            var clienteActivo = TestDataHelper.CreateSampleCliente(1);
            var clienteInactivo = TestDataHelper.CreateSampleCliente(2);
            clienteActivo.Activo = true;
            clienteInactivo.Activo = false;
            
            await _context.Set<Cliente>().AddRangeAsync(clienteActivo, clienteInactivo);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _clienteRepo.ObtenerActivos();

            // Assert
            resultado.Should().HaveCount(1);
            resultado.Should().OnlyContain(c => c.Activo);
        }

        [Fact]
        public async Task ObtenerPorTipo_DeberiaRetornarClientesDelTipo()
        {
            // Arrange
            var clienteJuridico = TestDataHelper.CreateSampleCliente(1);
            var clienteNatural = TestDataHelper.CreateSampleCliente(2);
            clienteJuridico.TipoCliente = "Jurídica";
            clienteNatural.TipoCliente = "Natural";
            
            await _context.Set<Cliente>().AddRangeAsync(clienteJuridico, clienteNatural);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _clienteRepo.ObtenerPorTipo("Jurídica");

            // Assert
            resultado.Should().HaveCount(1);
            resultado.Should().OnlyContain(c => c.TipoCliente == "Jurídica");
        }

        [Fact]
        public async Task ExisteNit_DeberiaRetornarTrue_CuandoExiste()
        {
            // Arrange
            var cliente = TestDataHelper.CreateSampleCliente(1);
            await _context.Set<Cliente>().AddAsync(cliente);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _clienteRepo.ExisteNit(cliente.Nit);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task ExisteNit_DeberiaRetornarFalse_CuandoNoExiste()
        {
            // Act
            var resultado = await _clienteRepo.ExisteNit("999999999");

            // Assert
            resultado.Should().BeFalse();
        }

        [Fact]
        public async Task ExisteCorreo_DeberiaRetornarTrue_CuandoExiste()
        {
            // Arrange
            var cliente = TestDataHelper.CreateSampleCliente(1);
            await _context.Set<Cliente>().AddAsync(cliente);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _clienteRepo.ExisteCorreo(cliente.CorreoContacto);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task ExisteCorreo_DeberiaRetornarFalse_CuandoNoExiste()
        {
            // Act
            var resultado = await _clienteRepo.ExisteCorreo("noexiste@test.com");

            // Assert
            resultado.Should().BeFalse();
        }

        [Fact]
        public async Task ObtenerPorPais_DeberiaRetornarClientesDelPais()
        {
            // Arrange
            var cliente1 = TestDataHelper.CreateSampleCliente(1);
            var cliente2 = TestDataHelper.CreateSampleCliente(2);
            cliente1.Pais = "Colombia";
            cliente2.Pais = "Colombia";
            
            await _context.Set<Cliente>().AddRangeAsync(cliente1, cliente2);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _clienteRepo.ObtenerPorPais("Colombia");

            // Assert
            resultado.Should().HaveCount(2);
            resultado.Should().OnlyContain(c => c.Pais == "Colombia");
        }

        [Fact]
        public async Task BuscarPorRazonSocial_DeberiaRetornarClientesQueCoincidan()
        {
            // Arrange
            var cliente1 = TestDataHelper.CreateSampleCliente(1);
            var cliente2 = TestDataHelper.CreateSampleCliente(2);
            cliente1.RazonSocial = "Empresa ABC Ltda";
            cliente2.RazonSocial = "Compañía ABC SAS";
            
            await _context.Set<Cliente>().AddRangeAsync(cliente1, cliente2);
            await _context.SaveChangesAsync();

            // Act
            var resultado = await _clienteRepo.BuscarPorRazonSocial("ABC");

            // Assert
            resultado.Should().HaveCount(2);
            resultado.Should().OnlyContain(c => c.RazonSocial.Contains("ABC"));
        }

        [Fact]
        public async Task ObtenerPorRangoFechas_DeberiaRetornarClientesEnElRango()
        {
            // Arrange
            var cliente1 = TestDataHelper.CreateSampleCliente(1);
            var cliente2 = TestDataHelper.CreateSampleCliente(2);
            cliente1.FechaRegistro = DateTime.Now.AddDays(-5);
            cliente2.FechaRegistro = DateTime.Now.AddDays(-10);
            
            await _context.Set<Cliente>().AddRangeAsync(cliente1, cliente2);
            await _context.SaveChangesAsync();

            var fechaInicio = DateTime.Now.AddDays(-15);
            var fechaFin = DateTime.Now.AddDays(-1);

            // Act
            var resultado = await _clienteRepo.ObtenerPorRangoFechas(fechaInicio, fechaFin);

            // Assert
            resultado.Should().HaveCount(2);
            resultado.Should().OnlyContain(c => c.FechaRegistro >= fechaInicio && c.FechaRegistro <= fechaFin);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
} 