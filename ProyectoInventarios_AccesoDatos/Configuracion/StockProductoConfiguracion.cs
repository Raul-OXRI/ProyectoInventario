using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProyectoInventarios_Modelos.ViewsModels;

namespace ProyectoInventarios_AccesoDatos.Configuracion
{
    public class StockProductoConfiguracion : IEntityTypeConfiguration<MStockProducto>
    {
        public void Configure(EntityTypeBuilder<MStockProducto> builder)
        {
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Cantidad).IsRequired();
            
            //------------------------------------------------------------------------

            builder.HasOne(x => x.Producto).WithMany()
                .HasForeignKey(x => x.ProductoId)
                .OnDelete(DeleteBehavior.NoAction);

            //------------------------------------------------------------------------
        }
    }
}
