using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoInventarios_Modelos.ViewsModels
{
    public class MProducto
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Numeor de serie es requerido")]
        
        public string NumeroSerie { get; set; }

        [Required(ErrorMessage = "Ingresar la descripción")]
        
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "Ingresar el precio")]
        
        public double Precio { get; set; }
        [Required(ErrorMessage = "Ingresar el costo")]
        
        public double Costo { get; set; }

        public string ImagenUrl {  get; set; }
        [Required(ErrorMessage = "El estado es requerido")]
        public bool Estado { get; set; }

        //---------------------------------------------------------

        [Required(ErrorMessage = "La categoria es requerida")]
        public int CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public MCategoria Categoria { get; set; }

        //-----------------------------------------------------------

        [Required(ErrorMessage = "La marca es requerida")]
        public int MarcaId { get; set; }
        [ForeignKey("MarcaId")]
        public Mmarca Marca { get; set; }

        //----------------------------------------------------------

        public int? PadreId { get; set; }
        [ForeignKey("PadreId")]
        public virtual MProducto Padre { get; set; }

        //----------------------------------------------------------
        public int BodegaId { get; set; }
        [ForeignKey("BodegaId")]
        public Mbodega Bodega { get; set; }

    }
}
