using System.ComponentModel.DataAnnotations;

namespace AlmaximoWEB.Models
{
    public class Proveedores
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [StringLength(255)]
        public string Descripcion { get; set; }

        // Relación con ProductoProveedor
        public virtual ICollection<ProductosProveedores> Productos { get; set; }
    }
}
