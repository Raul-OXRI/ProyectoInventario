using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using ProyectoInventarios_AccesoDatos.Data;
using ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_AccesoDatos.Repositorio
{
    public class UnidadTrabajo : IUnidadTrabajo
    {
        //crear variable para la realcion de ina info al appcontex
        private readonly ApplicationDbContext _context;

        public IBodegaRepositorio Bodega { get; private set; }
        public ICategoriaRepositorio Categoria { get; private set; }
        public IMarcaRepositorio Marca { get; private set; }
        public IProductoRepositorio Producto { get; private set; }
        public ICarritoRepositorio Carrito { get; private set; }
        public IRepositorio<IdentityUser> Usuario { get; private set; }
        public IStockProductoRepositorio StockProducto { get; private set; }


        //public IBodegaRepositorio BodegaRepositorio { get; private set; }

        public UnidadTrabajo(ApplicationDbContext context)
        {
            _context = context;
            Bodega = new BodegaRepositorio(_context);
            Categoria = new CategoriaRepositorio(_context);
            Marca = new MarcaRepositorio(_context);
            Producto = new ProductoRepositorio(_context);
            Carrito = new CarritoRepositorio(_context);
            Usuario = new Repositorio<IdentityUser>(_context);
            StockProducto = new StockProductoRepositorio(_context);
        }



        public void Dispose()
        {
            _context.Dispose();//librea memoria que no estamos utilziando
        }

        public async Task Guardar()
        {
            await _context.SaveChangesAsync(); //guarda los comabias y los podemos referenciar
        }

        public async Task<IDbContextTransaction> IniciarTransaccionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
    }
}
