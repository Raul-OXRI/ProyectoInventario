using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoInventarios_Modelos.ViewsModels;

namespace ProyectoInventarios_AccesoDatos.Configuracion
{
    public class ProductoConfiguracion : IEntityTypeConfiguration<MProducto>
    {
        public void Configure(EntityTypeBuilder<MProducto> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.NumeroSerie).IsRequired().HasMaxLength(60);
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Estado).IsRequired();
            builder.Property(x => x.Precio).IsRequired();
            builder.Property(x => x.Costo).IsRequired();
            builder.Property(x => x.CategoriaId).IsRequired();
            builder.Property(x => x.MarcaId).IsRequired();
            builder.Property(x => x.BodegaId).IsRequired();
            builder.Property(x => x.ImagenUrl).IsRequired();
            builder.Property(x => x.PadreId);
            
            //-------------------------------------------------------------------------

            builder.HasOne(x => x.Categoria).WithMany()
                .HasForeignKey(x => x.CategoriaId)
                .OnDelete(DeleteBehavior.NoAction);

            //------------------------------------------------------------------------

            builder.HasOne(x => x.Marca).WithMany()
                .HasForeignKey(x => x.MarcaId)
                .OnDelete(DeleteBehavior.NoAction);

            //-------------------------------------------------------------------------

            builder.HasOne(x => x.Padre).WithMany()
                .HasForeignKey(x => x.PadreId)
                .OnDelete(DeleteBehavior.NoAction);

            //--------------------------------------------------------------------------

            builder.HasOne(x => x.Bodega).WithMany()
                .HasForeignKey(x => x.BodegaId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
