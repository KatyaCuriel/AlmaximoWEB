using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlmaximoWEB.Models
{
    public class Producto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Clave { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        
        [Required(ErrorMessage = "Debe seleccionar un tipo de producto.")]
        public int? TipoProductoId { get; set; }

        public bool EsActivo { get; set; } = true;

        public decimal? Precio { get; set; }

        // Relación con TipoProducto
        [ForeignKey("TipoProductoId")]
        public virtual TipoProducto TipoProducto { get; set; }
      

        // Relación con Proveedores
        public virtual ICollection<ProductosProveedores> ProductosProveedores { get; set; }
    }
}
