using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio;
using ProyectoInventarios_Modelos.ViewsModels;
using ProyectoInventarios_Utilidades;
using System.Security.Claims;
using QuestPDF.Helpers;
using System.Reflection.Metadata;
using QuestPDF.Fluent;




namespace ProyectoInventarios.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BodegaController : Controller
    {
        //instanc a unidad de trabajo 
        private readonly IUnidadTrabajo _unidadTrabajo;

        public BodegaController(IUnidadTrabajo unidadTrabajo)
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
            Mbodega bodega = new Mbodega();
            if (id == null)
            {
                bodega.Estado = true;
                return View(bodega);
            }
            else
            {
                bodega = await _unidadTrabajo.Bodega.Obtener(id.GetValueOrDefault());
                if(bodega == null)
                {
                    return NotFound();
                }
                return View(bodega);
            }
        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upssert(Mbodega bodega)
        {
            if (ModelState.IsValid)
            {
                if ((bodega.Id == 0))
                {
                    await _unidadTrabajo.Bodega.Agregar(bodega);
                    TempData[DefinicionesEstaticas.Exitosa] = "Bodega Creada existisamente";
                }
                else
                {
                    _unidadTrabajo.Bodega.Actualizar(bodega);
                    TempData[DefinicionesEstaticas.Exitosa] = "Bodega Actualizada existisamente";
                }
                await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DefinicionesEstaticas.Error] = "hubo un error al guarda el registro";
            return View(bodega);
        }

        //------------------------------ Reporte producto ------------------------------

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GenerarReportePdf(int id)
        {
            var bodega = await _unidadTrabajo.Bodega.ObtenerPrimero(b => b.Id == id);

            if (bodega == null)
            {
                TempData["Error"] = "La bodega no existe.";
                return RedirectToAction("Index");
            }

            var productos = await _unidadTrabajo.Producto.ObtenerTodos(p => p.BodegaId == id);

            if (!productos.Any())
            {
                TempData["Error"] = $"No hay productos registrados en la bodega {bodega.Nombre}.";
                return RedirectToAction("Index");
            }

            // Generate PDF
            var pdfBytes = GenerateBodegaPDF(bodega, productos);
            return File(pdfBytes, "application/pdf", $"Reporte_Bodega_{bodega.Nombre}.pdf");
        }





        private byte[] GenerateBodegaPDF(ProyectoInventarios_Modelos.ViewsModels.Mbodega bodega, IEnumerable<ProyectoInventarios_Modelos.ViewsModels.MProducto> productos)
        {
            // Generate the PDF and return the byte array
            return QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);

                    // Correct alignment by applying it to the container
                    page.Header().AlignCenter().Text($"Reporte de Productos - {bodega.Nombre}").FontSize(20).Bold();

                    page.Content().Table(table =>
                    {
                        // Definir columnas
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(); // Número de Serie
                            columns.RelativeColumn(); // Descripción
                            columns.RelativeColumn(); // Precio
                            columns.RelativeColumn(); // Costo
                        });

                        // Cabecera de la tabla
                        table.Header(header =>
                        {
                            header.Cell().Text("Número de Serie").Bold();
                            header.Cell().Text("Descripción").Bold();
                            header.Cell().Text("Precio").Bold();
                            header.Cell().Text("Costo").Bold();
                        });

                        // Filas de la tabla
                        foreach (var producto in productos)
                        {
                            table.Cell().Text(producto.NumeroSerie);
                            table.Cell().Text(producto.Descripcion);
                            table.Cell().Text($"Q. {producto.Precio:0.00}");
                            table.Cell().Text($"Q. {producto.Costo:0.00}");
                        }

                        // Footer de la tabla
                        table.Footer(footer =>
                        {
                            footer.Cell().ColumnSpan(3).AlignRight().Text("Total de Productos:").Bold();
                            footer.Cell().Text($"{productos.Count()}").Bold();
                        });
                    });

                    page.Footer().AlignRight().Text($"Generado el {DateTime.Now:dd/MM/yyyy}").FontSize(10);
                });
            }).GeneratePdf(); // Ensure the byte array is returned here
        }










        //--------------------------------------------------------------------------------------------

        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var todos = await _unidadTrabajo.Bodega.ObtenerTodos(); 
            return Json(new { data = todos});
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var bodebaDB = await _unidadTrabajo.Bodega.Obtener(id);
            if (bodebaDB == null)
            {
                return Json(new { success = false, message = "Error al borrar la Bodega" });
            }
            _unidadTrabajo.Bodega.Eliminar(bodebaDB);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Bodega borrada exitosamente" });
        }

        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool existeNombre;
            string nombreNormalizado = nombre.ToLower().Trim();

            var lista = await _unidadTrabajo.Bodega.ObtenerTodos();
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
