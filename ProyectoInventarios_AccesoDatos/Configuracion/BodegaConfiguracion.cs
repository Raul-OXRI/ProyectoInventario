using Microsoft.EntityFrameworkCore;
using ProyectoInventarios_Modelos.ViewsModels;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_AccesoDatos.Configuracion
{
    public class BodegaConfiguracion : IEntityTypeConfiguration<Mbodega>
    {
        public void Configure(EntityTypeBuilder<Mbodega> builder)
        {
            //propiedades del modelo bodega uno por uno para realizar el overide y la aplicaion de fluent API
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Nombre).IsRequired().HasMaxLength(64);
            builder.Property(x => x.Descripcion).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Estado).IsRequired();
        }
    }
}
