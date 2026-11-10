using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.Api.Data;
using Pedidos.Api.Models;

namespace Pedidos.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PedidoController : ControllerBase
{
    private readonly PedidosDBContext _context;

    public PedidoController(PedidosDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pedido>>> GetPedidos()
    {
        var pedidos = await _context.Pedidos.AsNoTracking().ToListAsync();
        return Ok(pedidos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Pedido>> GetPedido(int id)
    {
        var pedido = await _context.Pedidos.AsNoTracking().FirstOrDefaultAsync(p => p.IdPedido == id);
        if (pedido == null)
        {
            return NotFound();
        }

        return Ok(pedido);
    }

    [HttpPost]
    public async Task<ActionResult<Pedido>> CreatePedido(Pedido pedido)
    {
        var clienteExiste = await _context.Clientes.AnyAsync(c => c.IdCliente == pedido.IdCliente);
        if (!clienteExiste)
        {
            return NotFound("El cliente especificado no existe");
        }

        if (pedido.Fecha == default)
        {
            pedido.Fecha = DateTime.UtcNow;
        }

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetPedido), new { id = pedido.IdPedido }, pedido);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePedido(int id, Pedido pedido)
    {
        if (id != pedido.IdPedido)
        {
            return BadRequest();
        }

        _context.Entry(pedido).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePedido(int id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido == null)
        {
            return NotFound();
        }

        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

