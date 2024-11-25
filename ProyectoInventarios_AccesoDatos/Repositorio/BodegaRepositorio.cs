using ProyectoInventarios_AccesoDatos.Data;
using ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio;
using ProyectoInventarios_Modelos.ViewsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_AccesoDatos.Repositorio
{
    public class BodegaRepositorio : Repositorio<Mbodega>, IBodegaRepositorio
    {
        //referencia a apliaccion db contex
        private readonly ApplicationDbContext _context;
        //baseconexts x medio de esta se envia la info a repositoio dad
        public BodegaRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }

        public void Actualizar(Mbodega mbodega)
        {
            var bodegaDb = _context.Bodega.FirstOrDefault(b => b.Id == mbodega.Id);
            if (bodegaDb != null)
            {
                bodegaDb.Nombre = mbodega.Nombre;
                bodegaDb.Descripcion = mbodega.Descripcion;
                bodegaDb.Estado = mbodega.Estado;
                _context.SaveChanges();
            }
        }
    }
}
