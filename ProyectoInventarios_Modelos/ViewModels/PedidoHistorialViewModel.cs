using Microsoft.AspNetCore.Mvc.Rendering;
using ProyectoInventarios_Modelos.ViewsModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace ProyectoInventarios_Modelos.ViewModels
{
    public class PedidoHistorialViewModel
    {
        public string UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public int TotalPedidos { get; set; }
        public decimal TotalPrecio { get; set; }
        public DateTime UltimoPedido { get; set; }
    }
}
