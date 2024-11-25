using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInventarios_Modelos.ViewsModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_Modelos.ViewModels
{
    public class ProductoFilter
    {
        // Productos filtrados que se mostrarán en la vista
        public IEnumerable<MProducto> ProductosFiltrados { get; set; } = new List<MProducto>();

        // Listas de selección para filtros
        public IEnumerable<SelectListItem> Categorias { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Marcas { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Bodegas { get; set; } = new List<SelectListItem>();

        // Valores seleccionados para los filtros
        [Display(Name = "Categoría Seleccionada")]
        public int? CategoriaSeleccionada { get; set; }

        [Display(Name = "Marca Seleccionada")]
        public int? MarcaSeleccionada { get; set; }

        [Display(Name = "Bodega Seleccionada")]
        public int? BodegaSeleccionada { get; set; }


    }
}
