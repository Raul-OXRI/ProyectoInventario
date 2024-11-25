using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInventarios_AccesoDatos.Data;
using ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio;
using ProyectoInventarios_Modelos.ViewsModels;

namespace ProyectoInventarios_AccesoDatos.Repositorio
{
    public class ProductoRepositorio : Repositorio<MProducto>, IProductoRepositorio
    {
        //referencia a apliaccion db contex
        private readonly ApplicationDbContext _context;
        //baseconexts x medio de esta se envia la info a repositoio dad
        public ProductoRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }

        public void Actualizar(MProducto mproducto)
        {
            var productoDb = _context.Produto.FirstOrDefault(b => b.Id == mproducto.Id);
            if (productoDb != null)
            {
                if (productoDb.ImagenUrl != null)
                {
                    productoDb.ImagenUrl = mproducto.ImagenUrl;
                }
                productoDb.NumeroSerie = mproducto.NumeroSerie;
                productoDb.Descripcion = mproducto.Descripcion;
                productoDb.Precio = mproducto.Precio;
                productoDb.Costo = mproducto.Costo;
                productoDb.CategoriaId = mproducto.CategoriaId;
                productoDb.MarcaId = mproducto.MarcaId;
                productoDb.PadreId = mproducto.PadreId;
                productoDb.Estado = mproducto.Estado;
                _context.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosLista(string obj)
        {

            if (obj == "Categoria")
            {
                return _context.Categorias
                    .Where(c => c.Estado == true)
                    .Select(c => new SelectListItem
                    {
                        Text = c.Nombre,
                        Value = c.Id.ToString()
                    });
            }
            if (obj == "Marca")
            {
                return _context.Marca
                    .Where(c => c.Estado == true)
                    .Select(c => new SelectListItem
                    {
                        Text = c.Nombre,
                        Value = c.Id.ToString()
                    });
            }
            if (obj == "Producto")
            {
                return _context.Produto
                    .Where(c => c.Estado == true)
                    .Select(c => new SelectListItem
                    {
                        Text = c.Descripcion,
                        Value = c.Id.ToString()
                    });
            }
            if (obj == "Bodega")
            {
                return _context.Bodega
                    .Where(c => c.Estado == true)
                    .Select(c => new SelectListItem
                    {
                        Text = c.Nombre,
                        Value = c.Id.ToString()
                    });
            }
            return null;
        }
    }
}
