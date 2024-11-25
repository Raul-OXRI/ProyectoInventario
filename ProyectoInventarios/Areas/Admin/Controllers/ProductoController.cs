using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio;
using ProyectoInventarios_Modelos.ViewModels;
using ProyectoInventarios_Modelos.ViewsModels;
using ProyectoInventarios_Utilidades;

namespace ProyectoInventarios.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductoController : Controller
    {

        //----------------------------------------------------
        //instanc a unidad de trabajo 
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductoController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upssert(int? id)
        {

            ProductoVM productoVM = new ProductoVM()
            {
                MProducto = new MProducto(),
                CategoriaLista = _unidadTrabajo.Producto.ObtenerTodosLista("Categoria"),
                MarcaLista = _unidadTrabajo.Producto.ObtenerTodosLista("Marca"),
                Padrelista = _unidadTrabajo.Producto.ObtenerTodosLista("Producto"),
                BodegaLista = _unidadTrabajo.Producto.ObtenerTodosLista("Bodega")
            };
            if (id == null)
            {
                return View(productoVM);
            }
            else
            {
                productoVM.MProducto = await _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
                if (productoVM.MProducto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }


        }


        //metodo post
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Upssert(ProductoVM productoVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;
                if (productoVM.MProducto.Id == 0)
                {
                    string upload = webRootPath + DefinicionesEstaticas.ImagenRuta;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload,fileName+extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productoVM.MProducto.ImagenUrl = fileName + extension;
                    await _unidadTrabajo.Producto.Agregar(productoVM.MProducto);
                }
                else
                {
                    var objProducto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == productoVM.MProducto.Id, isTracking:false);
                    if (files.Count > 0)
                    {
                        string upload = webRootPath + DefinicionesEstaticas.ImagenRuta;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var anteriorFile = Path.Combine(upload,objProducto.ImagenUrl);

                        if (System.IO.File.Exists(anteriorFile))
                        {
                            System.IO.File.Delete(anteriorFile);
                        }
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productoVM.MProducto.ImagenUrl = fileName + extension;
                    }
                    else
                    {
                        productoVM.MProducto.ImagenUrl = objProducto.ImagenUrl;
                    }
                    _unidadTrabajo.Producto.Actualizar(productoVM.MProducto);
                }
                TempData[DefinicionesEstaticas.Exitosa] = "Tranasaccion exitosa";
                await _unidadTrabajo.Guardar();
                return RedirectToAction("Index");
            }
            productoVM.CategoriaLista = _unidadTrabajo.Producto.ObtenerTodosLista("Cartegoria");
            productoVM.MarcaLista = _unidadTrabajo.Producto.ObtenerTodosLista("Marca");
            productoVM.Padrelista = _unidadTrabajo.Producto.ObtenerTodosLista("Producto");
            productoVM.BodegaLista = _unidadTrabajo.Producto.ObtenerTodosLista("Bodega");
            return View(productoVM);
        }

        //-------------------- Agregar cantidad producto --------------------------

        [HttpPost]
        public async Task<IActionResult> AgregarCantidad([FromBody] MStockProducto stockProducto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    using (var transaccion = await _unidadTrabajo.IniciarTransaccionAsync())
                    {
                        var producto = await _unidadTrabajo.Producto.ObtenerPrimero(p => p.Id == stockProducto.ProductoId);
                        if (producto == null)
                        {
                            return NotFound(new { mensaje = "Producto no encontrado." });
                        }

                        var nuevoStock = new MStockProducto
                        {
                            ProductoId = stockProducto.ProductoId,
                            Cantidad = stockProducto.Cantidad
                        };

                        _unidadTrabajo.StockProducto.Agregar(nuevoStock);
                        await _unidadTrabajo.Guardar();
                        await transaccion.CommitAsync();
                    }

                    return Ok(new { mensaje = "Cantidad agregada correctamente." });
                }

                return BadRequest(new { mensaje = "Datos inválidos.", errores = ModelState.Values });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error interno del servidor.", detalle = ex.Message });
            }
        }






        //-------------------------------------------------------------------------


        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Producto.ObtenerTodos(incluirPropedades: "Categoria,Marca");
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var productoDB = await _unidadTrabajo.Producto.Obtener(id);
            if (productoDB == null)
            {
                return Json(new { success = false, message = "Error al borrar el producto" });
            }
            //eliminar imagen 
            string upload = _webHostEnvironment.WebRootPath + DefinicionesEstaticas.ImagenRuta;
            var anteriorFile = Path.Combine(upload, productoDB.ImagenUrl);
            if (System.IO.File.Exists(anteriorFile))
            {
                System.IO.File.Delete(anteriorFile);
            }

            _unidadTrabajo.Producto.Eliminar(productoDB);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Producto borrada exitosamente" });
        }

        [ActionName("ValidarSerie")]
        public async Task<IActionResult> ValidarNombre(string serie, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Producto.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.NumeroSerie.ToLower().Trim() == serie.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b => b.NumeroSerie.ToLower().Trim() == serie.ToLower().Trim() && b.Id != id);
            }

            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { success = false });
        }


        #endregion
        //---------------------------------------------------

    }
}
