using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInventarios_Modelos.ViewsModels;

namespace ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio
{
    public interface IProductoRepositorio : IRepositorio<MProducto>
    {
        //acyualizar indivual 
        void Actualizar(MProducto mproducto);

        //metodo para obtner las listas
        IEnumerable<SelectListItem> ObtenerTodosLista(String obj);


    }
}
