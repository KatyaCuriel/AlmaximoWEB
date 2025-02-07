using AlmaximoWEB.Data;
using AlmaximoWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlmaximoWEB.Controllers
{
    public class ProductoController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Ver la lista de productos (Vista)
        public async Task<IActionResult> Index(string clave, int? tipoProductoId)
        {
            // Obtener los productos de la base de datos, con filtros si se proporcionan
            var productos = _context.Productos.Include(p => p.TipoProducto).AsQueryable();

            if (!string.IsNullOrEmpty(clave))
            {
                productos = productos.Where(p => p.Clave.Contains(clave));
            }

            if (tipoProductoId.HasValue)
            {
                productos = productos.Where(p => p.TipoProductoId == tipoProductoId);
            }

            // Pasar los productos filtrados a la vista
            ViewBag.TiposProducto = await _context.TiposProducto.ToListAsync(); // Pasa los tipos de productos al formulario
            return View(await productos.ToListAsync());
        }

        // ✅ Ver producto por ID (Vista)
        public async Task<IActionResult> Details(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.TipoProducto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // ✅ Editar producto
        public async Task<IActionResult> Edit(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.ProductosProveedores)  // Carga la relación con ProductoProveedor
                .ThenInclude(pp => pp.Proveedor)      // Carga los datos del Proveedor
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                return NotFound();
            }

            ViewBag.TiposProducto = await _context.TiposProducto.ToListAsync();
            ViewBag.Proveedores = await _context.Proveedores.ToListAsync(); // Lista de proveedores disponibles

            return View(producto);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, Producto producto)
        {
            if (id != producto.Id)
            {
                return BadRequest();
            }

            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // ✅ Eliminar producto
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Create()
        {
            // Pasar los tipos de productos a la vista para el dropdown
            ViewBag.TiposProducto = await _context.TiposProducto.ToListAsync();
            return View();
           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("El modelo del producto no es válido.");
                return View(producto);
            }

            try
            {
                // Verificar conexión con la base de datos antes de agregar el producto
                if (!await _context.Database.CanConnectAsync())
                {
                    Console.WriteLine("No se pudo establecer conexión con la base de datos.");
                    ModelState.AddModelError("", "Error de conexión con la base de datos.");
                    return View(producto);
                }

                Console.WriteLine("Conexión a la base de datos exitosa.");

                // Agregar el producto a la base de datos
                await _context.AddAsync(producto);
                Console.WriteLine($"Estado del producto antes de guardar: {_context.Entry(producto).State}");

                // Guardar cambios en la base de datos
                await _context.SaveChangesAsync();
                Console.WriteLine("Producto guardado correctamente.");

                return RedirectToAction(nameof(Index)); // Redirigir a la lista de productos
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al guardar el producto: {ex.Message}");
                ModelState.AddModelError("", "Hubo un error al guardar el producto. Intente nuevamente.");
                return View(producto);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AgregarProveedor(int productoId, int proveedorId, string claveProductoProveedor, decimal costo)
        {
            var productoProveedor = new ProductosProveedores
            {
                ProductoId = productoId,
                ProveedorId = proveedorId,
                ClaveProductoProveedor = claveProductoProveedor,
                Costo = costo
            };

            _context.ProductosProveedores.Add(productoProveedor);
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", new { id = productoId });
        }

        [HttpPost]
        public async Task<IActionResult> EditarProveedor(int id, int productoId, string claveProductoProveedor, decimal costo)
        {
            var productoProveedor = await _context.ProductosProveedores.FindAsync(id);
            if (productoProveedor == null)
            {
                return NotFound();
            }

            productoProveedor.ClaveProductoProveedor = claveProductoProveedor;
            productoProveedor.Costo = costo;

            _context.Entry(productoProveedor).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", new { id = productoId });
        }

        [HttpPost]
        public async Task<IActionResult> EliminarProveedor(int id, int productoId)
        {
            var productoProveedor = await _context.ProductosProveedores.FindAsync(id);
            if (productoProveedor == null)
            {
                return NotFound();
            }

            _context.ProductosProveedores.Remove(productoProveedor);
            await _context.SaveChangesAsync();

            return RedirectToAction("Edit", new { id = productoId });
        }




    }
}
