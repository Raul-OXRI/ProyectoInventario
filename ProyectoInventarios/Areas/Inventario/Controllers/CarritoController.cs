using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProyectoInventarios_AccesoDatos.Repositorio.IRepositorio;
using ProyectoInventarios_Modelos.ViewModels;
using System.Security.Claims;
using QuestPDF.Fluent;
using QuestPDF.Helpers;


namespace ProyectoInventarios.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class CarritoController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;

        public CarritoController(IUnidadTrabajo unidadTrabajo)
        {
            _unidadTrabajo = unidadTrabajo;

        }

        [Authorize(Roles = "User")]
        public async Task<IActionResult> Index()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioId))
            {
                TempData["Error"] = "Debe iniciar sesión para ver su carrito.";
                return RedirectToAction("Index", "Home");
            }
            var carrito = await _unidadTrabajo.Carrito.ObtenerTodosConProductoAsync();

            var carritoUsuario = carrito.Where(c => c.UsuarioId == usuarioId && !c.Confirmacion);
            return View(carritoUsuario);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ConfirmarPedido()
        {
            var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(usuarioId))
            {
                TempData["Error"] = "Debe iniciar sesión para confirmar su pedido.";
                return RedirectToAction("Index");
            }

            // Obtener productos del carrito del usuario
            var carritos = (await _unidadTrabajo.Carrito.ObtenerTodosConProductoAsync())
                .Where(c => c.UsuarioId == usuarioId && !c.Confirmacion)
                .ToList();

            if (!carritos.Any())
            {
                TempData["Error"] = "No hay productos para confirmar.";
                return RedirectToAction("Index");
            }

            foreach (var carrito in carritos)
            {
                // Obtener todos los registros de StockProducto relacionados con el ProductoId
                var stockProductos = await _unidadTrabajo.StockProducto.ObtenerTodosAsync(sp => sp.ProductoId == carrito.ProductoId);

                if (stockProductos == null || !stockProductos.Any())
                {
                    TempData["Error"] = $"El producto {carrito.Producto?.Descripcion} no tiene stock registrado.";
                    return RedirectToAction("Index");
                }

                int cantidadRestante = carrito.Cantidad;

                foreach (var stock in stockProductos.OrderBy(sp => sp.Id)) // Procesar en orden
                {
                    if (cantidadRestante <= 0)
                        break;

                    if (stock.Cantidad >= cantidadRestante)
                    {
                        // Consumir solo lo necesario
                        stock.Cantidad -= cantidadRestante;
                        cantidadRestante = 0;
                    }
                    else
                    {
                        // Consumir todo el stock disponible en este registro
                        cantidadRestante -= stock.Cantidad;
                        stock.Cantidad = 0;
                    }

                    // Actualizar el registro de stock
                    _unidadTrabajo.StockProducto.Actualizar(stock);
                }

                if (cantidadRestante > 0)
                {
                    TempData["Error"] = $"No hay suficiente stock para el producto: {carrito.Producto?.Descripcion}.";
                    return RedirectToAction("Index");
                }

                // Marcar el carrito como confirmado
                carrito.Confirmacion = true;
                _unidadTrabajo.Carrito.Actualizar(carrito);
            }

            // Guardar cambios en la base de datos
            await _unidadTrabajo.Guardar();

            // Generar PDF de confirmación
            var pdf = GenerateOrderPDF(carritos);
            return File(pdf, "application/pdf", "Pedido_Confirmado.pdf");
        }






        private byte[] GenerateOrderPDF(IEnumerable<ProyectoInventarios_Modelos.ViewsModels.MCarrito> carritos)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, QuestPDF.Infrastructure.Unit.Centimetre);
                    page.Header().Text("Resumen del Pedido").FontSize(20).Bold();
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                            columns.RelativeColumn();
                        });
                        table.Header(header =>
                        {
                            header.Cell().Text("Producto").Bold();
                            header.Cell().Text("Cantidad").Bold();
                            header.Cell().Text("Precio").Bold();
                            header.Cell().Text("Total").Bold();
                        });
                        foreach (var item in carritos)
                        {
                            table.Cell().Text(item.Producto?.Descripcion ?? "N/A");
                            table.Cell().Text(item.Cantidad.ToString());
                            table.Cell().Text($"Q. {item.Precio:0.00}");
                            table.Cell().Text($"Q. {item.Total:0.00}");
                        }
                        table.Footer(footer =>
                        {
                            footer.Cell().ColumnSpan(3).AlignRight().Text("Total:").Bold();
                            footer.Cell().Text($"Q. {carritos.Sum(c => c.Total):0.00}").Bold();
                        });
                    });
                });
            }).GeneratePdf();
        }

        //-------------------------------------------------------------------------------------------------


        public async Task<IActionResult> Historial()
        {
            var pedidosConfirmados = await _unidadTrabajo.Carrito.ObtenerTodos();
            var pedidos = pedidosConfirmados
                .Where(c => c.Confirmacion)
                .GroupBy(c => c.UsuarioId)
                .Select(grupo => new PedidoHistorialViewModel
                {
                    UsuarioId = grupo.Key,
                    NombreUsuario = grupo.FirstOrDefault()?.Usuario?.UserName ?? "Usuario no encontrado",
                    TotalPedidos = grupo.Sum(c => c.Cantidad),
                    TotalPrecio = grupo.Sum(c => c.Total),
                    UltimoPedido = grupo.Max(c => c.FechaAgregado)
                }).ToList();

            return View(pedidos);
        }

        public async Task<IActionResult> PedidosConfirmados()
        {
            var pedidosConfirmados = await _unidadTrabajo.Carrito.ObtenerPedidosConfirmadosConUsuarioAsync();
            var historialPedidos = pedidosConfirmados
                .GroupBy(p => new { p.UsuarioId, p.Usuario?.UserName })
                .Select(g => new PedidoHistorialViewModel
                {
                    UsuarioId = g.Key.UsuarioId,
                    NombreUsuario = g.Key.UserName ?? "Usuario no encontrado",
                    TotalPedidos = g.Count(),
                    TotalPrecio = g.Sum(p => p.Precio * p.Cantidad),
                    UltimoPedido = g.Max(p => p.FechaAgregado)
                })
                .ToList();
            return View(historialPedidos);
        }


        //-------------------------PDF--------------------------------------------------

        public async Task<IActionResult> GenerarPdfPorUsuario(string usuarioId)
        {
            try
            {
                Console.WriteLine("Iniciando generación del PDF...");
                Console.WriteLine($"UsuarioId: {usuarioId}");
                var pedidosUsuario = await _unidadTrabajo.Carrito.ObtenerTodos();
                if (pedidosUsuario == null || !pedidosUsuario.Any())
                {
                    Console.WriteLine("No se encontraron registros en la tabla Carrito.");
                    TempData["Error"] = "No se encontraron registros en la tabla Carrito.";
                    return RedirectToAction("PedidosConfirmados");
                }
                var pedidosFiltrados = pedidosUsuario
                    .Where(c => c.UsuarioId == usuarioId && c.Confirmacion)
                    .ToList();
                if (!pedidosFiltrados.Any())
                {
                    TempData["Error"] = "No hay pedidos confirmados para este usuario.";
                    return RedirectToAction("PedidosConfirmados");
                }
                var productos = await _unidadTrabajo.Producto.ObtenerTodos();
                var pedidosConProductos = pedidosFiltrados.Select(pedido => new
                {
                    Pedido = pedido,
                    Producto = productos.FirstOrDefault(prod => prod.Id == pedido.ProductoId)
                }).ToList();
                foreach (var item in pedidosConProductos)
                {
                    if (item.Producto == null)
                    {
                        Console.WriteLine($"Producto no encontrado para ProductoId={item.Pedido.ProductoId}");
                        TempData["Error"] = "Algunos productos no se encontraron en el inventario.";
                        return RedirectToAction("PedidosConfirmados");
                    }
                }
                var pdfDocument = Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(1, QuestPDF.Infrastructure.Unit.Centimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(12).FontFamily("Arial"));
                        page.Header().Text($"Historial de compras confirmadas para el usuario: {usuarioId}")
                            .Bold()
                            .FontSize(16)
                            .FontColor(Colors.Blue.Medium);
                        page.Content().Stack(stack =>
                        {
                            stack.Spacing(5);

                            foreach (var item in pedidosConProductos)
                            {
                                stack.Item().BorderBottom(1).PaddingBottom(5).Row(row =>
                                {
                                    row.RelativeItem().Text($"Producto: {item.Producto.Descripcion}").Bold();
                                    row.RelativeItem().Text($"Precio: Q{item.Producto.Precio:0.00}");
                                    row.RelativeItem().Text($"Cantidad: {item.Pedido.Cantidad}");
                                    row.RelativeItem().Text($"Total: Q{item.Producto.Precio * item.Pedido.Cantidad:0.00}");
                                    row.RelativeItem().Text($"Fecha: {item.Pedido.FechaAgregado:dd/MM/yyyy HH:mm:ss}");
                                });
                            }
                        });
                        page.Footer().AlignCenter().Text(x =>
                        {
                            x.Span("Generado el: ");
                            x.Span($"{DateTime.Now:dd/MM/yyyy HH:mm:ss}").Bold();
                        });
                    });
                });
                byte[] pdfBytes;
                using (var memoryStream = new MemoryStream())
                {
                    pdfDocument.GeneratePdf(memoryStream);
                    pdfBytes = memoryStream.ToArray();
                }

                if (pdfBytes == null || pdfBytes.Length == 0)
                {
                    TempData["Error"] = "Error al generar el PDF.";
                    return RedirectToAction("PedidosConfirmados");
                }
                return File(pdfBytes, "application/pdf", $"Historial_{usuarioId}.pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al generar el PDF: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                TempData["Error"] = $"Ocurrió un error al generar el PDF: {ex.Message}";
                return RedirectToAction("PedidosConfirmados");
            }

        }

        //-------------------------------------------------------------------------------------------------




    }
}
