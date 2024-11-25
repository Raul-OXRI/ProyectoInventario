using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_Modelos.ViewsModels
{
    public class Mbodega
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Ingresar el nombre de la bodega")]
        [MaxLength(60, ErrorMessage = "El nombre debe ser como máximmo de 60 carácteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "Ingresar la descripción de la bodega")]
        [MaxLength(100, ErrorMessage = "La descripción debe ser como máximmo de 100 carácteres")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Se debe agregar el estado de la boedga")]
        public bool Estado { get; set; }

    } 
}
