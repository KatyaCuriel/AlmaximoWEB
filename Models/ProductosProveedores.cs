using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlmaximoWEB.Models
{
    public class ProductosProveedores
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ProductoId { get; set; }

        [Required]
        public int ProveedorId { get; set; }

        [Required]
        [StringLength(50)]
        public string ClaveProductoProveedor { get; set; }

        [Required]
        public decimal Costo { get; set; }

        // Relaciones con Producto y Proveedor
        [ForeignKey("ProductoId")]
        public virtual Producto Producto { get; set; }

        [ForeignKey("ProveedorId")]
        public virtual Proveedores Proveedor { get; set; }
    }
}
