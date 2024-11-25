using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio;
using ProyectoInventarios_Modelos.ViewsModels;
using ProyectoInventarios_Utilidades;

namespace ProyectoInventarios.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MarcaController : Controller
    {
        //instanc a unidad de trabajo 
        private readonly IUnidadTrabajo _unidadTrabajo;

        public MarcaController(IUnidadTrabajo unidadTrabajo)
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
            Mmarca marca = new Mmarca();
            if (id == null)
            {
                marca.Estado = true;
                return View(marca);
            }
            else
            {
                marca = await _unidadTrabajo.Marca.Obtener(id.GetValueOrDefault());
                if (marca == null)
                {
                    return NotFound();
                }
                return View(marca);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upssert(Mmarca marca)
        {
            if (ModelState.IsValid)
            {
                if ((marca.Id == 0))
                {
                    await _unidadTrabajo.Marca.Agregar(marca);
                    TempData[DefinicionesEstaticas.Exitosa] = "Marca Creada existisamente";
                }
                else
                {
                    _unidadTrabajo.Marca.Actualizar(marca);
                    TempData[DefinicionesEstaticas.Exitosa] = "Marca Actualizada existisamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DefinicionesEstaticas.Error] = "hubo un error al guarda el registro";
            return View(marca);
        }

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Marca.ObtenerTodos();
            return Json(new { data = todos });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var marcaDB = await _unidadTrabajo.Marca.Obtener(id);
            if (marcaDB == null)
            {
                return Json(new { success = false, message = "Error al borrar la marca" });
            }
            _unidadTrabajo.Marca.Eliminar(marcaDB);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "marca borrada exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool existeNombre;
            string nombreNormalizado = nombre.ToLower().Trim();

            var lista = await _unidadTrabajo.Marca.ObtenerTodos();
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
