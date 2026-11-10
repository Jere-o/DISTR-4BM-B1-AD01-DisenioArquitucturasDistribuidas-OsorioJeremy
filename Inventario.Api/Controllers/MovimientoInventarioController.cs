using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventario.Api.Data;
using Inventario.Api.Models;

namespace Inventario.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MovimientoInventarioController : ControllerBase
{
    private readonly InventarioDBContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public MovimientoInventarioController(InventarioDBContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MovimientoInventario>>> GetMovimientos()
    {
        var movimientos = await _context.MovimientosInventario.AsNoTracking().ToListAsync();
        return Ok(movimientos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MovimientoInventario>> GetMovimiento(int id)
    {
        var movimiento = await _context.MovimientosInventario.AsNoTracking().FirstOrDefaultAsync(m => m.IdMovimiento == id);
        if (movimiento == null)
        {
            return NotFound();
        }

        return Ok(movimiento);
    }

    [HttpPost]
    public async Task<ActionResult<MovimientoInventario>> CreateMovimiento(MovimientoInventario movimiento)
    {
        var bodegaExiste = await _context.Bodegas.AnyAsync(b => b.IdBodega == movimiento.IdBodega);
        if (!bodegaExiste)
        {
            return NotFound("La bodega especificada no existe");
        }

        if (movimiento.Cantidad <= 0)
        {
            return BadRequest("La cantidad debe ser mayor a 0");
        }

        var client = _httpClientFactory.CreateClient();
        var catalogoUrl = _configuration["CatalogoApiUrl"];
        if (!string.IsNullOrEmpty(catalogoUrl))
        {
            try
            {
                var response = await client.GetAsync($"{catalogoUrl}/api/Producto/{movimiento.IdProducto}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound("El producto especificado no existe en el catálogo");
                }
            }
            catch (HttpRequestException)
            {
            }
        }

        if (movimiento.Fecha == default)
        {
            movimiento.Fecha = DateTime.UtcNow;
        }

        _context.MovimientosInventario.Add(movimiento);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMovimiento), new { id = movimiento.IdMovimiento }, movimiento);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMovimiento(int id, MovimientoInventario movimiento)
    {
        if (id != movimiento.IdMovimiento)
        {
            return BadRequest();
        }

        if (movimiento.Cantidad <= 0)
        {
            return BadRequest("La cantidad debe ser mayor a 0");
        }

        _context.Entry(movimiento).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMovimiento(int id)
    {
        var movimiento = await _context.MovimientosInventario.FindAsync(id);
        if (movimiento == null)
        {
            return NotFound();
        }

        _context.MovimientosInventario.Remove(movimiento);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

