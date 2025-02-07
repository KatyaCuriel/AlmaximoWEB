using AlmaximoWEB.Data;
using AlmaximoWEB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AlmaximoWEB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoProductoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TipoProductoController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoProducto>>> GetTiposProducto()
        {
            return await _context.TiposProducto.ToListAsync();
        }
    }
}
