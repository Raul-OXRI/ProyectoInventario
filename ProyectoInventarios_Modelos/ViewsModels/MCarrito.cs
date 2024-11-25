using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_Modelos.ViewsModels
{
    public class MCarrito
    {
        [Key]
        public int Id { get; set; }
        //---------------------------------------------------------------------
        [Required(ErrorMessage = "El producto es requerida")]
        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public virtual MProducto Producto { get; set; }
        //---------------------------------------------------------------------
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Total => Precio * Cantidad;

        public string Direccion { get; set; }
        public string Tel { get; set; }
        public int CodigoPostal { get; set; }
        public string Departamento { get; set; }
        public string Municipio { get; set; }
        public DateTime FechaAgregado { get; set; } = DateTime.Now;
        public bool Confirmacion { set; get; }

        public string UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public virtual IdentityUser Usuario { get; set; }

    }
}
