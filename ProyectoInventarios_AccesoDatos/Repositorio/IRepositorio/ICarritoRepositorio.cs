using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInventarios_Modelos.ViewsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio
{
    public interface ICarritoRepositorio : IRepositorio<MCarrito>
    {
        //acyualizar indivual 
        void Actualizar(MCarrito mcarrito);

        //metodo para obtner las listas
        IEnumerable<SelectListItem> ObtenerTodosLista(String obj);
        Task<IEnumerable<MCarrito>> ObtenerTodosConProductoAsync();
        Task<IEnumerable<MCarrito>> ObtenerPedidosConfirmadosConUsuarioAsync();
        

    }
}
