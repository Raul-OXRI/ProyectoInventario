﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio
{
    public interface IRepositorio<T> where T : class
    {
        Task<T> Obtener(int id);
        Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string incluirPropedades = null,
            bool isTracking = true);
        Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null,
            string incluirPropedades = null,
            bool isTracking = true);
        Task Agregar(T entidad);
        void Eliminar(T entidad);
        void EliminarRango(IEnumerable<T> entidad);

    }
}
