using App.Interfaces.Repos.Generic;
using Domain.Entities;

namespace App.Interfaces.Repos
{
    /// <summary>
    /// Interfaz para el repositorio de Cliente que define operaciones específicas además de las genéricas.
    /// </summary>
    public interface IClienteRepo : IGenericRepo<Cliente>
    {
        
    }
}
