using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventario.Api.Data;
using Inventario.Api.Models;

namespace Inventario.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReservaStockController : ControllerBase
{
    private readonly InventarioDBContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public ReservaStockController(InventarioDBContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservaStock>>> GetReservas()
    {
        var reservas = await _context.ReservasStock.AsNoTracking().ToListAsync();
        return Ok(reservas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ReservaStock>> GetReserva(int id)
    {
        var reserva = await _context.ReservasStock.AsNoTracking().FirstOrDefaultAsync(r => r.IdReserva == id);
        if (reserva == null)
        {
            return NotFound();
        }

        return Ok(reserva);
    }

    [HttpPost]
    public async Task<ActionResult<ReservaStock>> CreateReserva(ReservaStock reserva)
    {
        var bodegaExiste = await _context.Bodegas.AnyAsync(b => b.IdBodega == reserva.IdBodega);
        if (!bodegaExiste)
        {
            return NotFound("La bodega especificada no existe");
        }

        if (reserva.Cantidad <= 0)
        {
            return BadRequest("La cantidad debe ser mayor a 0");
        }

        var client = _httpClientFactory.CreateClient();
        var catalogoUrl = _configuration["CatalogoApiUrl"];
        if (!string.IsNullOrEmpty(catalogoUrl))
        {
            try
            {
                var response = await client.GetAsync($"{catalogoUrl}/api/Producto/{reserva.IdProducto}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound("El producto especificado no existe en el catálogo");
                }
            }
            catch (HttpRequestException)
            {
            }
        }

        _context.ReservasStock.Add(reserva);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetReserva), new { id = reserva.IdReserva }, reserva);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateReserva(int id, ReservaStock reserva)
    {
        if (id != reserva.IdReserva)
        {
            return BadRequest();
        }

        if (reserva.Cantidad <= 0)
        {
            return BadRequest("La cantidad debe ser mayor a 0");
        }

        _context.Entry(reserva).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteReserva(int id)
    {
        var reserva = await _context.ReservasStock.FindAsync(id);
        if (reserva == null)
        {
            return NotFound();
        }

        _context.ReservasStock.Remove(reserva);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

