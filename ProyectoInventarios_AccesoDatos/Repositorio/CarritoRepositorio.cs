using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
    public class CarritoRepositorio : Repositorio<MCarrito>, ICarritoRepositorio
    {
        //referencia a apliaccion db contex
        private readonly ApplicationDbContext _context;
        //baseconexts x medio de esta se envia la info a repositoio dad
        public CarritoRepositorio(ApplicationDbContext context) : base(context)
        {
            _context = context;

        }

        public void Actualizar(MCarrito mcarrito)
        {
            var carritoDb = _context.Carrito.FirstOrDefault(b => b.Id == mcarrito.Id);
            if (carritoDb != null)
            {

                carritoDb.Precio = mcarrito.Precio;
                carritoDb.Cantidad = mcarrito.Cantidad;
                carritoDb.Direccion = mcarrito.Direccion;
                carritoDb.Tel = mcarrito.Tel;
                carritoDb.CodigoPostal = mcarrito.CodigoPostal;
                carritoDb.Departamento = mcarrito.Departamento;
                carritoDb.Municipio = mcarrito.Municipio;
                carritoDb.FechaAgregado = mcarrito.FechaAgregado;
                carritoDb.ProductoId = mcarrito.ProductoId;
                carritoDb.UsuarioId = mcarrito.UsuarioId;
                
                carritoDb.Confirmacion = mcarrito.Confirmacion;
                _context.SaveChanges();
            }
        }

        public IEnumerable<SelectListItem> ObtenerTodosLista(string obj)
        {
            if (obj == "Producto")
            {
                return _context.Produto
                    .Where(c => c.Estado == true)
                    .Select(c => new SelectListItem
                    {
                        Text = c.Descripcion,
                        Value = c.Id.ToString()
                    });
            }
            if (obj == "Usuario")
            {
                return _context.Users
                    .Select(c => new SelectListItem
                    {
                        Text = c.UserName,
                        Value = c.Id.ToString()
                    });
            }
            
            return null;
        }
        public async Task<IEnumerable<MCarrito>> ObtenerTodosConProductoAsync()
        {
            return await _context.Carrito
                .Include(c => c.Producto) // Incluir la relación con Producto
                .ToListAsync();
        }
        public async Task<IEnumerable<MCarrito>> ObtenerPedidosConfirmadosConUsuarioAsync()
        {
            var pedidos = await _context.Carrito
                .Include(c => c.Usuario) // Incluir la relación con IdentityUser o ApplicationUser
                .Where(c => c.Confirmacion == true)
                .ToListAsync();
            foreach (var pedido in pedidos)
            {
                Console.WriteLine($"CarritoId: {pedido.Id}, UsuarioId: {pedido.UsuarioId}, Usuario: {pedido.Usuario?.UserName}");
            }

            return pedidos;
        }

        

    }
}
