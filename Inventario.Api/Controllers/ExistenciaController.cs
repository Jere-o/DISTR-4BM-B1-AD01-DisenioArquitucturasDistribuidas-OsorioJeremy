using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventario.Api.Data;
using Inventario.Api.Models;

namespace Inventario.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExistenciaController : ControllerBase
{
    private readonly InventarioDBContext _context;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public ExistenciaController(InventarioDBContext context, IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _context = context;
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Existencia>>> GetExistencias()
    {
        var existencias = await _context.Existencias.AsNoTracking().ToListAsync();
        return Ok(existencias);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Existencia>> GetExistencia(int id)
    {
        var existencia = await _context.Existencias.AsNoTracking().FirstOrDefaultAsync(e => e.IdExistencia == id);
        if (existencia == null)
        {
            return NotFound();
        }

        return Ok(existencia);
    }

    [HttpPost]
    public async Task<ActionResult<Existencia>> CreateExistencia(Existencia existencia)
    {
        var bodegaExiste = await _context.Bodegas.AnyAsync(b => b.IdBodega == existencia.IdBodega);
        if (!bodegaExiste)
        {
            return NotFound("La bodega especificada no existe");
        }

        if (existencia.Stock < 0)
        {
            return BadRequest("El stock debe ser mayor o igual a 0");
        }

        var client = _httpClientFactory.CreateClient();
        var catalogoUrl = _configuration["CatalogoApiUrl"];
        if (!string.IsNullOrEmpty(catalogoUrl))
        {
            try
            {
                var response = await client.GetAsync($"{catalogoUrl}/api/Producto/{existencia.IdProducto}");
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return NotFound("El producto especificado no existe en el catálogo");
                }
            }
            catch (HttpRequestException)
            {
            }
        }

        _context.Existencias.Add(existencia);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetExistencia), new { id = existencia.IdExistencia }, existencia);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateExistencia(int id, Existencia existencia)
    {
        if (id != existencia.IdExistencia)
        {
            return BadRequest();
        }

        if (existencia.Stock < 0)
        {
            return BadRequest("El stock debe ser mayor o igual a 0");
        }

        _context.Entry(existencia).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteExistencia(int id)
    {
        var existencia = await _context.Existencias.FindAsync(id);
        if (existencia == null)
        {
            return NotFound();
        }

        _context.Existencias.Remove(existencia);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

