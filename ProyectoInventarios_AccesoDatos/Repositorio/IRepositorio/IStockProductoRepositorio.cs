using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInventarios_Modelos.ViewsModels;
using System.Linq.Expressions;

namespace ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio
{
    public interface IStockProductoRepositorio : IRepositorio<MStockProducto>
    {
        //acyualizar indivual 
        void Actualizar(MStockProducto mstockproducto);

        //metodo para obtner las listas
        IEnumerable<SelectListItem> ObtenerTodosLista(String obj);

        Task<MStockProducto> ObtenerPrimeroAsync(Expression<Func<MStockProducto, bool>> filtro);

        Task<IEnumerable<MStockProducto>> ObtenerTodosAsync(Expression<Func<MStockProducto, bool>> filtro);
    }
}
