using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Mapping
{
    public class ClienteConfiguration : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {

            builder.ToTable("cliente", "public");

            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("id");

            builder.Property(c => c.RazonSocial).HasColumnName("razon_social").IsRequired().HasMaxLength(150);
            builder.Property(c => c.Nit).HasColumnName("nit").IsRequired().HasMaxLength(50);
            builder.Property(c => c.TipoCliente).HasColumnName("tipo_cliente").IsRequired().HasMaxLength(50);
            builder.Property(c => c.RepresentanteLegal).HasColumnName("representante_legal").IsRequired().HasMaxLength(150);
            builder.Property(c => c.CorreoContacto).HasColumnName("correo_contacto").IsRequired().HasMaxLength(100);
            builder.Property(c => c.TelefonoContacto).HasColumnName("telefono_contacto").IsRequired().HasMaxLength(50);
            builder.Property(c => c.Direccion).HasColumnName("direccion").IsRequired().HasMaxLength(200);
            builder.Property(c => c.Ciudad).HasColumnName("ciudad").IsRequired().HasMaxLength(100);
            builder.Property(c => c.Pais).HasColumnName("pais").IsRequired().HasMaxLength(100);
            builder.Property(c => c.PaginaWeb).HasColumnName("pagina_web").HasMaxLength(150);
            builder.Property(c => c.Notas).HasColumnName("notas").HasMaxLength(500);
            builder.Property(c => c.Activo).HasColumnName("activo");
            builder.Property(c => c.FechaRegistro).HasColumnName("fecha_registro").HasColumnType("timestamp");

        }
    }
}
