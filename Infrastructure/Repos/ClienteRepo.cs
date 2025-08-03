using App.Interfaces.Repos;
using Domain.Entities;
using Infrastructure.Repos.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repos
{
    public class ClienteRepo : GenericRepo<Cliente>, IClienteRepo
    {
        private readonly DbContexto _context;

        public ClienteRepo(DbContexto context) : base(context)
        {
            _context = context;
        }

        public async Task<Cliente> ObtenerPorNit(string nit)
        {
            return await _context.Set<Cliente>()
                .FirstOrDefaultAsync(c => c.Nit == nit);
        }

        public async Task<Cliente> ObtenerPorCorreo(string correo)
        {
            return await _context.Set<Cliente>()
                .FirstOrDefaultAsync(c => c.CorreoContacto == correo);
        }

        public async Task<List<Cliente>> ObtenerPorCiudad(string ciudad)
        {
            return await _context.Set<Cliente>()
                .Where(c => c.Ciudad == ciudad)
                .ToListAsync();
        }

        public async Task<List<Cliente>> ObtenerActivos()
        {
            return await _context.Set<Cliente>()
                .Where(c => c.Activo)
                .ToListAsync();
        }

        public async Task<List<Cliente>> ObtenerPorTipo(string tipoCliente)
        {
            return await _context.Set<Cliente>()
                .Where(c => c.TipoCliente == tipoCliente)
                .ToListAsync();
        }

        public async Task<bool> ExisteNit(string nit)
        {
            return await _context.Set<Cliente>()
                .AnyAsync(c => c.Nit == nit);
        }

        public async Task<bool> ExisteCorreo(string correo)
        {
            return await _context.Set<Cliente>()
                .AnyAsync(c => c.CorreoContacto == correo);
        }

        public async Task<List<Cliente>> ObtenerPorPais(string pais)
        {
            return await _context.Set<Cliente>()
                .Where(c => c.Pais == pais)
                .ToListAsync();
        }

        public async Task<List<Cliente>> BuscarPorRazonSocial(string razonSocial)
        {
            return await _context.Set<Cliente>()
                .Where(c => c.RazonSocial.Contains(razonSocial))
                .ToListAsync();
        }

        public async Task<List<Cliente>> ObtenerPorRangoFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.Set<Cliente>()
                .Where(c => c.FechaRegistro >= fechaInicio && c.FechaRegistro <= fechaFin)
                .ToListAsync();
        }
    }
}
