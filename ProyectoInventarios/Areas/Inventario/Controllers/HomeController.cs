using Microsoft.AspNetCore.Mvc;
using ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio;
using ProyectoInventarios_Modelos.ViewsModels;
using ProyectoInventarios_Modelos.ViewModels;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProyectoInventarios.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnidadTrabajo _unidadTrabajo;

        public HomeController(ILogger<HomeController> logger, IUnidadTrabajo unidadTrabajo)
        {
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task<IActionResult> Index(int? bodegaId)
        {
            // Obtener todos los productos o filtrarlos por bodega si se seleccionó una
            var productos = await _unidadTrabajo.Producto.ObtenerTodos(
                filtro: bodegaId.HasValue ? p => p.BodegaId == bodegaId.Value : null,
                incluirPropedades: "Bodega");

            // Obtener todas las bodegas para el filtro
            var bodegas = await _unidadTrabajo.Bodega.ObtenerTodos();

            // Pasar las bodegas como ViewBag (para mantener el modelo simple)
            ViewBag.Bodegas = bodegas.Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Nombre // Cambia 'Nombre' si tu modelo Mbodega tiene otra propiedad para el nombre
            }).ToList();

            return View(productos);
        }



        //------------------------------------------------------------------

        [Authorize(Roles = "User")]
        public async Task<IActionResult> AgregarAlCarrito(int productoId)
        {
            var producto = await _unidadTrabajo.Producto.Obtener(productoId);
            if (producto != null)
            {
                await AgregarOIncrementarCarrito(productoId);
                return RedirectToAction("Index", "Carrito", new { area = "Inventario" });
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AgregarOIncrementarCarrito(int productoId)
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioId))
            {
                TempData["Error"] = "Debe iniciar sesión para agregar productos al carrito.";
                return RedirectToAction("Index");
            }
            var producto = await _unidadTrabajo.Producto.Obtener(productoId);
            if (producto != null)
            {
                var carritoExistente = await _unidadTrabajo.Carrito.ObtenerTodos();
                var carritoProducto = carritoExistente.FirstOrDefault(c => c.ProductoId == productoId && c.UsuarioId == usuarioId && !c.Confirmacion);
                if (carritoProducto != null)
                {
                    carritoProducto.Cantidad++;
                    _unidadTrabajo.Carrito.Actualizar(carritoProducto);
                }
                else
                {
                    var carrito = new MCarrito
                    {
                        ProductoId = producto.Id,
                        UsuarioId = usuarioId,
                        Precio = Convert.ToDecimal(producto.Precio),
                        Cantidad = 1,
                        FechaAgregado = DateTime.Now,
                        Confirmacion = false,
                        Direccion = "Dirección predeterminada",
                        Tel = "0000000000",
                        CodigoPostal = 0,
                        Departamento = "Departamento no especificado",
                        Municipio = "Municipio no especificado"
                    };
                    await _unidadTrabajo.Carrito.Agregar(carrito);
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction("Index", "Carrito", new { area = "Inventario" });
            }
            TempData["Error"] = "El producto seleccionado no existe.";
            return RedirectToAction("Index");
        }

        //------------------------------------------------------------------


        



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
