using ProyectoInventarios_Modelos.ViewsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio
{
    public interface IMarcaRepositorio : IRepositorio<Mmarca>
    {
        //acyualizar indivual 
        void Actualizar(Mmarca mmarca);


    }
}
