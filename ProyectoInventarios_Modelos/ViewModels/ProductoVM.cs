using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInventarios_Modelos.ViewsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_Modelos.ViewModels
{
    public class ProductoVM
    {
        public MProducto MProducto { get; set; }
        public IEnumerable<SelectListItem> CategoriaLista { get; set; }
        public IEnumerable<SelectListItem> MarcaLista { get; set; }
        public IEnumerable<SelectListItem> Padrelista { get; set; }
        public IEnumerable<SelectListItem> ProductoLista { get; set; }
        public IEnumerable<SelectListItem> Usualista { get; set; }
        public IEnumerable<SelectListItem> BodegaLista { get; set; }

    }
}
