using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_Modelos.ViewsModels
{
    public class MStockProducto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Ingresar la cantidad a comprar")]
        public int Cantidad { get; set; }
        //-----------------------------------------------------------

        [Required(ErrorMessage = "El producto es requerido")]
        public int ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public MProducto Producto { get; set; }

        //-----------------------------------------------------------
    }
}
