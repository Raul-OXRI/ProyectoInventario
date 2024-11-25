using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio
{
    public interface IUnidadTrabajo : IDisposable
    {
        IBodegaRepositorio Bodega { get; }
        ICategoriaRepositorio Categoria { get; }
        IMarcaRepositorio Marca { get; }
        IProductoRepositorio Producto { get; }
        ICarritoRepositorio Carrito { get; }
        IRepositorio<IdentityUser> Usuario { get; }
        IStockProductoRepositorio StockProducto { get; }

        Task Guardar();
        Task<IDbContextTransaction> IniciarTransaccionAsync();
    }
}
