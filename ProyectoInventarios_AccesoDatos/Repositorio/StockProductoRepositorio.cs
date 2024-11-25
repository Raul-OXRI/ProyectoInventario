using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProyectoInventarios_AccesoDatos.Data;
using ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio;
using ProyectoInventarios_Modelos.ViewsModels;
using System.Linq.Expressions;

namespace ProyectoInventarios_AccesoDatos.Repositorio
{
    public class StockProductoRepositorio : Repositorio<MStockProducto>, IStockProductoRepositorio
    {
        //referencia a apliaccion db contex
        private readonly ApplicationDbContext _context;
        //baseconexts x medio de esta se envia la info a repositoio dad
        public StockProductoRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }

        public void Actualizar(MStockProducto mstockproducto)
        {
            var stockproductoDb = _context.StockProducto.FirstOrDefault(b => b.Id == mstockproducto.Id);
            if (stockproductoDb != null)
            {
                stockproductoDb.Cantidad = mstockproducto.Cantidad;
                _context.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosLista(string obj)
        {

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
            return null;
        }

        public async Task<MStockProducto> ObtenerPrimeroAsync(Expression<Func<MStockProducto, bool>> filtro)
        {
            return await _context.StockProducto.FirstOrDefaultAsync(filtro);
        }

        public async Task<IEnumerable<MStockProducto>> ObtenerTodosAsync(Expression<Func<MStockProducto, bool>> filtro)
        {
            return await _context.StockProducto.Where(filtro).ToListAsync();
        }
    }
}
