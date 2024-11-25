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
    public class MarcaRepositorio : Repositorio<Mmarca>, IMarcaRepositorio
    {
        //referencia a apliaccion db contex
        private readonly ApplicationDbContext _context;
        //baseconexts x medio de esta se envia la info a repositoio dad
        public MarcaRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }

        public void Actualizar(Mmarca mmarca)
        {
            var marcaDb = _context.Bodega.FirstOrDefault(b => b.Id == mmarca.Id);
            if (marcaDb != null)
            {
                marcaDb.Nombre = mmarca.Nombre;
                marcaDb.Descripcion = mmarca.Descripcion;
                marcaDb.Estado = mmarca.Estado;
                _context.SaveChanges();
            }
        }
    }
}
