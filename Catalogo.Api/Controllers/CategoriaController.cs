using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Catalogo.Api.Data;
using Catalogo.Api.Models;

namespace Catalogo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriaController : ControllerBase
{
    private readonly CatalogoDBContext _context;

    public CategoriaController(CatalogoDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
    {
        var categorias = await _context.Categorias.AsNoTracking().ToListAsync();
        return Ok(categorias);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Categoria>> GetCategoria(int id)
    {
        var categoria = await _context.Categorias.AsNoTracking().FirstOrDefaultAsync(c => c.IdCategoria == id);
        if (categoria == null)
        {
            return NotFound();
        }

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult<Categoria>> CreateCategoria(Categoria categoria)
    {
        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetCategoria), new { id = categoria.IdCategoria }, categoria);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCategoria(int id, Categoria categoria)
    {
        if (id != categoria.IdCategoria)
        {
            return BadRequest();
        }

        _context.Entry(categoria).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategoria(int id)
    {
        var categoria = await _context.Categorias.FindAsync(id);
        if (categoria == null)
        {
            return NotFound();
        }

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
