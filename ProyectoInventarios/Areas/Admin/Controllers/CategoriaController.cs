using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio;
using ProyectoInventarios_Modelos.ViewsModels;
using ProyectoInventarios_Utilidades;

namespace ProyectoInventarios.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriaController : Controller
    {
        //instanc a unidad de trabajo 
        private readonly IUnidadTrabajo _unidadTrabajo;

        public CategoriaController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upssert(int? id)
        {
            MCategoria categoria = new MCategoria();
            if (id == null)
            {
                categoria.Estado = true;
                return View(categoria);
            }
            else
            {
                categoria = await _unidadTrabajo.Categoria.Obtener(id.GetValueOrDefault());
                if(categoria == null)
                {
                    return NotFound();
                }
                return View(categoria);
            }
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upssert(MCategoria categoria)
        {
            if (ModelState.IsValid)
            {
                if ((categoria.Id == 0))
                {
                    await _unidadTrabajo.Categoria.Agregar(categoria);
                    TempData[DefinicionesEstaticas.Exitosa] = "Categoria Creada existisamente";
                }
                else
                {
                    _unidadTrabajo.Categoria.Actualizar(categoria);
                    TempData[DefinicionesEstaticas.Exitosa] = "Categoria Actualizada existisamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DefinicionesEstaticas.Error] = "hubo un error al guarda el registro";
            return View(categoria);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Categoria.ObtenerTodos(); 
            return Json(new { data = todos});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var categoriaDB = await _unidadTrabajo.Categoria.Obtener(id);
            if (categoriaDB == null)
            {
                return Json(new { success = false, message = "Error al borrar la categoria" });
            }
            _unidadTrabajo.Categoria.Eliminar(categoriaDB);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Categoria borrada exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool existeNombre;
            string nombreNormalizado = nombre.ToLower().Trim();

            var lista = await _unidadTrabajo.Categoria.ObtenerTodos();
            if (id == 0)
            {
                existeNombre = lista.Any(b => b.Nombre.ToLower().Trim() == nombreNormalizado);
            }
            else
            {
                existeNombre = lista.Any(b => b.Nombre.ToLower().Trim() == nombreNormalizado && b.Id != id);
            }

            return Json(new { data = existeNombre });
        }


        #endregion
    }
}
