using AlmaximoWEB.Data;
using AlmaximoWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AlmaximoWEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProveedoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ OBTENER TODOS LOS PROVEEDORES
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Proveedores>>> GetProveedores()
        {
            try
            {
                var proveedores = await _context.Proveedores.Include(p => p.Productos).ToListAsync();

                if (proveedores == null || !proveedores.Any())
                {
                    return NotFound(new { message = "No hay proveedores disponibles" });
                }

                return Ok(proveedores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error en el servidor", error = ex.Message });
            }
        }

        // ✅ OBTENER UN PROVEEDOR POR ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Proveedores>> GetProveedor(int id)
        {
            try
            {
                var proveedor = await _context.Proveedores.Include(p => p.Productos).FirstOrDefaultAsync(p => p.Id == id);

                if (proveedor == null)
                {
                    return NotFound(new { message = $"Proveedor con ID {id} no encontrado" });
                }

                return Ok(proveedor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error en el servidor", error = ex.Message });
            }
        }

        // ✅ AGREGAR UN NUEVO PROVEEDOR
        [HttpPost]
        public async Task<ActionResult<Proveedores>> AddProveedor([FromBody] Proveedores proveedor)
        {
            try
            {
                if (proveedor == null)
                {
                    return BadRequest(new { message = "Los datos del proveedor son inválidos" });
                }

                _context.Proveedores.Add(proveedor);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProveedor), new { id = proveedor.Id }, proveedor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al agregar el proveedor", error = ex.Message });
            }
        }

        // ✅ EDITAR UN PROVEEDOR EXISTENTE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProveedor(int id, [FromBody] Proveedores proveedor)
        {
            try
            {
                if (id != proveedor.Id)
                {
                    return BadRequest(new { message = "El ID del proveedor no coincide" });
                }

                var proveedorExistente = await _context.Proveedores.FindAsync(id);
                if (proveedorExistente == null)
                {
                    return NotFound(new { message = $"Proveedor con ID {id} no encontrado" });
                }

                proveedorExistente.Nombre = proveedor.Nombre;
                proveedorExistente.Descripcion = proveedor.Descripcion;

                _context.Entry(proveedorExistente).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al actualizar el proveedor", error = ex.Message });
            }
        }

        // ✅ ELIMINAR UN PROVEEDOR
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProveedor(int id)
        {
            try
            {
                var proveedor = await _context.Proveedores.FindAsync(id);
                if (proveedor == null)
                {
                    return NotFound(new { message = $"Proveedor con ID {id} no encontrado" });
                }

                _context.Proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error al eliminar el proveedor", error = ex.Message });
            }
        }
    }
}
