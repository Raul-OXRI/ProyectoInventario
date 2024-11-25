using Microsoft.EntityFrameworkCore;
using ProyectoInventarios_AccesoDatos.Data;
using ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_AccesoDatos.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        //relacion con el DB contex
        private readonly ApplicationDbContext _context;
        internal DbSet<T> dbSet;
        //cosntuctor
        public Repositorio(ApplicationDbContext context)
        {
            _context = context;
            this.dbSet = _context.Set<T>();
        }
        //--------------------------
        public async Task Agregar(T entidad)
        {
            await dbSet.AddAsync(entidad); //insert a la entidad
        }

        public void Eliminar(T entidad)
        {
            dbSet.Remove(entidad);//elimianr un registro
        }

        public void EliminarRango(IEnumerable<T> entidad)
        {
            dbSet.RemoveRange(entidad);//eliminar un grupo
        }

        public async Task<T> Obtener(int id)
        {
            return await dbSet.FindAsync(id);//selct for fltrer for id
        }

        public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null, string incluirPropedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if(filtro == null)
            {
                query = query.Where(filtro);
            }
            if(incluirPropedades != null)
            {
                foreach(var incluirProp in incluirPropedades.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                }
            }
            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropedades = null, bool isTracking = true)
        {
            IQueryable<T> query = dbSet;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            if (incluirPropedades != null)
            {
                foreach (var incluirProp in incluirPropedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            if (!isTracking)
            {
                query = query.AsNoTracking();
            }
            return await query.ToListAsync();
        }
    }
}
