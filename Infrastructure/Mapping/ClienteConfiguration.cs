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

            builder.Property(c => c.Id)
                .HasColumnName("id");

            builder.Property(c => c.Nombre)
                .HasColumnName("nombre")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(c => c.DocumentoIdentidad)
                .HasColumnName("documento_identidad")
                .HasMaxLength(50);

            builder.Property(c => c.TipoCliente)
                .HasColumnName("tipo_cliente")
                .HasMaxLength(50);

            builder.Property(c => c.Correo)
                .HasColumnName("correo")
                .HasMaxLength(100);

            builder.Property(c => c.Telefono)
                .HasColumnName("telefono")
                .HasMaxLength(50);

            builder.Property(c => c.Empresa)
                .HasColumnName("empresa")
                .HasMaxLength(100);

            builder.Property(c => c.Direccion)
                .HasColumnName("direccion");

            builder.Property(c => c.Ciudad)
                .HasColumnName("ciudad")
                .HasMaxLength(100);

            builder.Property(c => c.Pais)
                .HasColumnName("pais")
                .HasMaxLength(100);

            builder.Property(c => c.SitioWeb)
                .HasColumnName("sitio_web")
                .HasMaxLength(200);

            builder.Property(c => c.NotasInternas)
                .HasColumnName("notas_internas");

            builder.Property(c => c.CreadoEn)
                .HasColumnName("creado_en")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
