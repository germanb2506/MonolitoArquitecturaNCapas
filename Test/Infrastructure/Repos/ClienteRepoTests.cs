using App.Interfaces.Repos;
using Domain.Entities;
using Infrastructure.Repos;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Test.Helpers;

namespace Test.Infrastructure.Repos
{
    public class ClienteRepoTests : IDisposable
    {
        private readonly DbContextOptions<DbContexto> _options;
        private readonly DbContexto _context;
        private readonly ClienteRepo _clienteRepo;

        public ClienteRepoTests()
        {
            _options = new DbContextOptionsBuilder<DbContexto>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new DbContexto(_options);
            _clienteRepo = new ClienteRepo(_context);
        }

        [Fact]
        public async Task Crear_DeberiaAgregarClienteALaBaseDeDatos()
        {
            // Arrange
            var cliente = TestDataHelper.CreateSampleCliente();

            // Act
            await _clienteRepo.Crear(cliente);
            await _context.SaveChangesAsync();

            // Assert
            var clienteGuardado = await _context.Set<Cliente>().FirstOrDefaultAsync(c => c.Id == cliente.Id);
            clienteGuardado.Should().NotBeNull();
            clienteGuardado!.RazonSocial.Should().Be(cliente.RazonSocial);
            clienteGuardado.Nit.Should().Be(cliente.Nit);
        }

        [Fact]
        public async Task Obtener_DeberiaRetornarCliente_CuandoExiste()
        {
            // Arrange
            var cliente = TestDataHelper.CreateSampleCliente();
            _context.Set<Cliente>().Add(cliente);
            await _context.SaveChangesAsync();

            // Act
            var clienteEncontrado = await _clienteRepo.Obtener(c => c.Id == cliente.Id);

            // Assert
            clienteEncontrado.Should().NotBeNull();
            clienteEncontrado!.Id.Should().Be(cliente.Id);
            clienteEncontrado.RazonSocial.Should().Be(cliente.RazonSocial);
        }

        [Fact]
        public async Task Obtener_DeberiaRetornarNull_CuandoNoExiste()
        {
            // Arrange
            var idInexistente = 999;

            // Act
            var clienteEncontrado = await _clienteRepo.Obtener(c => c.Id == idInexistente);

            // Assert
            clienteEncontrado.Should().BeNull();
        }

        [Fact]
        public async Task ObtenerTodos_DeberiaRetornarTodosLosClientes()
        {
            // Arrange
            var clientes = TestDataHelper.CreateSampleClientes(3);
            _context.Set<Cliente>().AddRange(clientes);
            await _context.SaveChangesAsync();

            // Act
            var clientesObtenidos = await _clienteRepo.ObtenerTodos();

            // Assert
            clientesObtenidos.Should().NotBeNull();
            clientesObtenidos.Count.Should().Be(3);
            clientesObtenidos.Should().BeEquivalentTo(clientes, options => options.Excluding(c => c.FechaRegistro));
        }

        [Fact]
        public async Task ObtenerTodos_DeberiaRetornarListaVacia_CuandoNoHayClientes()
        {
            // Act
            var clientesObtenidos = await _clienteRepo.ObtenerTodos();

            // Assert
            clientesObtenidos.Should().NotBeNull();
            clientesObtenidos.Should().BeEmpty();
        }

        [Fact]
        public async Task Remover_DeberiaEliminarClienteDeLaBaseDeDatos()
        {
            // Arrange
            var cliente = TestDataHelper.CreateSampleCliente();
            _context.Set<Cliente>().Add(cliente);
            await _context.SaveChangesAsync();

            // Act
            await _clienteRepo.Remover(cliente);
            await _context.SaveChangesAsync();

            // Assert
            var clienteEliminado = await _context.Set<Cliente>().FirstOrDefaultAsync(c => c.Id == cliente.Id);
            clienteEliminado.Should().BeNull();
        }

        [Fact]
        public async Task Grabar_DeberiaGuardarCambiosEnLaBaseDeDatos()
        {
            // Arrange
            var cliente = TestDataHelper.CreateSampleCliente();
            _context.Set<Cliente>().Add(cliente);

            // Act
            await _clienteRepo.Grabar();

            // Assert
            var clienteGuardado = await _context.Set<Cliente>().FirstOrDefaultAsync(c => c.Id == cliente.Id);
            clienteGuardado.Should().NotBeNull();
        }

        [Fact]
        public async Task Obtener_DeberiaFiltrarPorCriterios()
        {
            // Arrange
            var clientes = new List<Cliente>
            {
                TestDataHelper.CreateSampleCliente(1),
                TestDataHelper.CreateSampleCliente(2)
            };
            clientes[0].Activo = true;
            clientes[1].Activo = false;

            _context.Set<Cliente>().AddRange(clientes);
            await _context.SaveChangesAsync();

            // Act
            var clientesActivos = await _clienteRepo.Obtener(c => c.Activo == true);

            // Assert
            clientesActivos.Should().NotBeNull();
            clientesActivos!.Activo.Should().BeTrue();
        }

        [Fact]
        public async Task Obtener_DeberiaRetornarPrimerClienteQueCumplaCriterio()
        {
            // Arrange
            var clientes = TestDataHelper.CreateSampleClientes(3);
            _context.Set<Cliente>().AddRange(clientes);
            await _context.SaveChangesAsync();

            // Act
            var primerCliente = await _clienteRepo.Obtener(c => c.Id > 0);

            // Assert
            primerCliente.Should().NotBeNull();
            primerCliente!.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Crear_DeberiaAsignarIdAutomaticamente()
        {
            // Arrange
            var cliente = TestDataHelper.CreateSampleCliente();
            cliente.Id = 0; // Reset ID

            // Act
            await _clienteRepo.Crear(cliente);
            await _context.SaveChangesAsync();

            // Assert
            cliente.Id.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task Obtener_DeberiaManejarExpresionesComplejas()
        {
            // Arrange
            var clientes = new List<Cliente>
            {
                TestDataHelper.CreateSampleCliente(1),
                TestDataHelper.CreateSampleCliente(2),
                TestDataHelper.CreateSampleCliente(3)
            };
            clientes[0].Ciudad = "Bogotá";
            clientes[1].Ciudad = "Medellín";
            clientes[2].Ciudad = "Bogotá";

            _context.Set<Cliente>().AddRange(clientes);
            await _context.SaveChangesAsync();

            // Act
            var clientesBogota = await _clienteRepo.Obtener(c => c.Ciudad == "Bogotá" && c.Activo == true);

            // Assert
            clientesBogota.Should().NotBeNull();
            clientesBogota!.Ciudad.Should().Be("Bogotá");
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
} 