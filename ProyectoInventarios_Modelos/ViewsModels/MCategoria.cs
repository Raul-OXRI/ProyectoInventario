using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_Modelos.ViewsModels
{
    public class MCategoria
    {
        [Key]

        public int Id { get; set; }

        [Required(ErrorMessage ="Nombre requerido")]
        [MaxLength(60, ErrorMessage ="El nombre de la ctegoria debe ser maximo de 60 carcateres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Descripcion requerido")]
        [MaxLength(100, ErrorMessage = "La Descripcion de la ctegoria debe ser maximo de 10 carcateres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El estado es requerido")]
        public bool Estado { get; set; }




    }
}
