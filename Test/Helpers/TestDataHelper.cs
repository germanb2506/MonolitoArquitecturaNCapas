using App.Dto;
using Domain.Entities;

namespace Test.Helpers
{
    public static class TestDataHelper
    {
        public static Cliente CreateSampleCliente(int id = 1)
        {
            return new Cliente
            {
                Id = id,
                RazonSocial = $"Empresa Test {id}",
                Nit = $"12345678-{id}",
                TipoCliente = "Jurídica",
                RepresentanteLegal = $"Juan Pérez {id}",
                CorreoContacto = $"contacto{id}@empresa.com",
                TelefonoContacto = $"300123456{id}",
                Direccion = $"Calle {id} # {id}-{id}",
                Ciudad = "Bogotá",
                Pais = "Colombia",
                PaginaWeb = $"www.empresa{id}.com",
                Notas = $"Notas de prueba para cliente {id}",
                Activo = true,
                FechaRegistro = DateTime.Now.AddDays(-(id % 1000))
            };
        }

        public static ClienteDto CreateSampleClienteDto(int id = 1)
        {
            return new ClienteDto
            {
                Id = id,
                RazonSocial = $"Empresa Test {id}",
                Nit = $"12345678-{id}",
                TipoCliente = "Jurídica",
                RepresentanteLegal = $"Juan Pérez {id}",
                CorreoContacto = $"contacto{id}@empresa.com",
                TelefonoContacto = $"300123456{id}",
                Direccion = $"Calle {id} # {id}-{id}",
                Ciudad = "Bogotá",
                Pais = "Colombia",
                PaginaWeb = $"www.empresa{id}.com",
                Notas = $"Notas de prueba para cliente {id}",
                Activo = true,
                FechaRegistro = DateTime.Now.AddDays(-(id % 1000))
            };
        }

        public static List<Cliente> CreateSampleClientes(int count = 3)
        {
            var clientes = new List<Cliente>();
            for (int i = 1; i <= count; i++)
            {
                clientes.Add(CreateSampleCliente(i));
            }
            return clientes;
        }

        public static List<ClienteDto> CreateSampleClienteDtos(int count = 3)
        {
            var clientesDto = new List<ClienteDto>();
            for (int i = 1; i <= count; i++)
            {
                clientesDto.Add(CreateSampleClienteDto(i));
            }
            return clientesDto;
        }
    }
} 