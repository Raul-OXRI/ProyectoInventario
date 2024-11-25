using ProyectoInventarios_Modelos.ViewsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio
{
    public interface ICategoriaRepositorio : IRepositorio<MCategoria>
    {
        //acyualizar indivual 
        void Actualizar(MCategoria mcategoria);


    }
}
