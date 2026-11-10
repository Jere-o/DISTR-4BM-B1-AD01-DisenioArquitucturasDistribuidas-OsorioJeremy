using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Catalogo.Api.Data;
using Catalogo.Api.Models;

namespace Catalogo.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductoController : ControllerBase
{
    private readonly CatalogoDBContext _context;

    public ProductoController(CatalogoDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Producto>>> GetProductos()
    {
        var productos = await _context.Productos.AsNoTracking().ToListAsync();
        return Ok(productos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Producto>> GetProducto(int id)
    {
        var producto = await _context.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.IdProducto == id);
        if (producto == null)
        {
            return NotFound();
        }

        return Ok(producto);
    }

    [HttpPost]
    public async Task<ActionResult<Producto>> CreateProducto(Producto producto)
    {
        var categoriaExiste = await _context.Categorias.AnyAsync(c => c.IdCategoria == producto.IdCategoria);
        var marcaExiste = await _context.Marcas.AnyAsync(m => m.IdMarca == producto.IdMarca);

        if (!categoriaExiste || !marcaExiste)
        {
            return BadRequest("La categoría o marca especificada no existe");
        }

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetProducto), new { id = producto.IdProducto }, producto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProducto(int id, Producto producto)
    {
        if (id != producto.IdProducto)
        {
            return BadRequest();
        }

        _context.Entry(producto).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProducto(int id)
    {
        var producto = await _context.Productos.FindAsync(id);
        if (producto == null)
        {
            return NotFound();
        }

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
