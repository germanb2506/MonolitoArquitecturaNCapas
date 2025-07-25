using Domain.Entities;
using Infrastructure.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class DbContexto : DbContext
    {
        public DbContexto(DbContextOptions<DbContexto> options) : base(options)
        {
        }
        // Define DbSets for your entities here
        public DbSet<Cliente> Clientes { get; set; }

        //Mapping de las entidades
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}

