using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Catalogo.Api.Data;
using Catalogo.Api.Models;

namespace Catalogo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class MarcaController : ControllerBase
{
    private readonly CatalogoDBContext _context;

    public MarcaController(CatalogoDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Marca>>> GetMarcas()
    {
        var marcas = await _context.Marcas.AsNoTracking().ToListAsync();
        return Ok(marcas);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Marca>> GetMarca(int id)
    {
        var marca = await _context.Marcas.AsNoTracking().FirstOrDefaultAsync(m => m.IdMarca == id);
        if (marca == null)
        {
            return NotFound();
        }

        return Ok(marca);
    }

    [HttpPost]
    public async Task<ActionResult<Marca>> CreateMarca(Marca marca)
    {
        _context.Marcas.Add(marca);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetMarca), new { id = marca.IdMarca }, marca);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMarca(int id, Marca marca)
    {
        if (id != marca.IdMarca)
        {
            return BadRequest();
        }

        _context.Entry(marca).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMarca(int id)
    {
        var marca = await _context.Marcas.FindAsync(id);
        if (marca == null)
        {
            return NotFound();
        }

        _context.Marcas.Remove(marca);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
