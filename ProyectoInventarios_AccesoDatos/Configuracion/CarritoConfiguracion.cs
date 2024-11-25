using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoInventarios_Modelos.ViewsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_AccesoDatos.Configuracion
{
    public class CarritoConfiguracion : IEntityTypeConfiguration<MCarrito>
    {
        public void Configure(EntityTypeBuilder<MCarrito> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Precio).IsRequired().HasColumnType("decimal(18,2)");
            builder.Property(x => x.Cantidad).IsRequired();
            builder.Property(x => x.Direccion).IsRequired();
            builder.Property(x => x.Tel).IsRequired();
            builder.Property(x => x.CodigoPostal).IsRequired();
            builder.Property(x => x.Departamento).IsRequired();
            builder.Property(x => x.Municipio).IsRequired();
            builder.Property(x => x.FechaAgregado).IsRequired().HasColumnType("datetime");
            builder.Property(x => x.Confirmacion).IsRequired();
            builder.HasOne(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Producto)
                  .WithMany()
                  .HasForeignKey(x => x.ProductoId)
                  .OnDelete(DeleteBehavior.Restrict);

            

            builder.Ignore(x => x.Total);
        }
    }
}
