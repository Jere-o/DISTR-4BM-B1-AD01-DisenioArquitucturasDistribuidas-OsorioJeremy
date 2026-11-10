using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Inventario.Api.Data;
using Inventario.Api.Models;

namespace Inventario.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BodegaController : ControllerBase
{
    private readonly InventarioDBContext _context;

    public BodegaController(InventarioDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Bodega>>> GetBodegas()
    {
        var bodegas = await _context.Bodegas.AsNoTracking().ToListAsync();
        return Ok(bodegas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Bodega>> GetBodega(int id)
    {
        var bodega = await _context.Bodegas.AsNoTracking().FirstOrDefaultAsync(b => b.IdBodega == id);
        if (bodega == null)
        {
            return NotFound();
        }

        return Ok(bodega);
    }

    [HttpPost]
    public async Task<ActionResult<Bodega>> CreateBodega(Bodega bodega)
    {
        _context.Bodegas.Add(bodega);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetBodega), new { id = bodega.IdBodega }, bodega);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBodega(int id, Bodega bodega)
    {
        if (id != bodega.IdBodega)
        {
            return BadRequest();
        }

        _context.Entry(bodega).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBodega(int id)
    {
        var bodega = await _context.Bodegas.FindAsync(id);
        if (bodega == null)
        {
            return NotFound();
        }

        _context.Bodegas.Remove(bodega);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

