using Microsoft.EntityFrameworkCore;
using AlmaximoWEB.Models;

namespace AlmaximoWEB.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Producto> Productos { get; set; }
        public DbSet<TipoProducto> TiposProducto { get; set; }
        public DbSet<Proveedores> Proveedores { get; set; }
        public DbSet<ProductosProveedores> ProductosProveedores { get; set; }

    }
}
