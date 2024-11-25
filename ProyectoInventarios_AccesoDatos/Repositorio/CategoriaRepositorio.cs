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
    public class CategoriaRepositorio : Repositorio<MCategoria>, ICategoriaRepositorio
    {
        //referencia a apliaccion db contex
        private readonly ApplicationDbContext _context;
        //baseconexts x medio de esta se envia la info a repositoio dad
        public CategoriaRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }

        public void Actualizar(MCategoria mcategoria)
        {
            var categoriaDb = _context.Bodega.FirstOrDefault(b => b.Id == mcategoria.Id);
            if (categoriaDb != null)
            {
                categoriaDb.Nombre = mcategoria.Nombre;
                categoriaDb.Descripcion = mcategoria.Descripcion;
                categoriaDb.Estado = mcategoria.Estado;
                _context.SaveChanges();
            }
        }
    }
}
