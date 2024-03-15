using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcCoreTiendaZapatillas.Models
{
    [Table("IMAGENESZAPASPRACTICA")]
    public class ImagenesZapatilla
    {
        [Key]
        [Column("IDIMAGEN")]
        public int IdImagen {  get; set; }
        [Column("IDPRODUCTO")]
        public int IdProducto { get; set; }
        [Column("IMAGEN")] 
        public string Imagen {  get; set; }

    }
}
