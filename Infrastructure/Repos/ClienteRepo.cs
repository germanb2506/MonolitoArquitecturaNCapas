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
        
    }
}
