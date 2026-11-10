using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pedidos.Api.Data;
using Pedidos.Api.Models;

namespace Pedidos.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DetallePedidoController : ControllerBase
{
    private readonly PedidosDBContext _context;

    public DetallePedidoController(PedidosDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DetallePedido>>> GetDetallesPedido()
    {
        var detalles = await _context.DetallesPedido.AsNoTracking().ToListAsync();
        return Ok(detalles);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DetallePedido>> GetDetallePedido(int id)
    {
        var detalle = await _context.DetallesPedido.AsNoTracking().FirstOrDefaultAsync(d => d.IdDetalle == id);
        if (detalle == null)
        {
            return NotFound();
        }

        return Ok(detalle);
    }

    [HttpPost]
    public async Task<ActionResult<DetallePedido>> CreateDetallePedido(DetallePedido detallePedido)
    {
        var pedidoExiste = await _context.Pedidos.AnyAsync(p => p.IdPedido == detallePedido.IdPedido);
        if (!pedidoExiste)
        {
            return BadRequest("El pedido especificado no existe");
        }

        if (detallePedido.Cantidad <= 0)
        {
            return BadRequest("La cantidad debe ser mayor a 0");
        }

        if (detallePedido.PrecioUnitario <= 0)
        {
            return BadRequest("El precio unitario debe ser mayor a 0");
        }

        _context.DetallesPedido.Add(detallePedido);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetDetallePedido), new { id = detallePedido.IdDetalle }, detallePedido);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateDetallePedido(int id, DetallePedido detallePedido)
    {
        if (id != detallePedido.IdDetalle)
        {
            return BadRequest();
        }

        if (detallePedido.Cantidad <= 0)
        {
            return BadRequest("La cantidad debe ser mayor a 0");
        }

        if (detallePedido.PrecioUnitario <= 0)
        {
            return BadRequest("El precio unitario debe ser mayor a 0");
        }

        _context.Entry(detallePedido).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteDetallePedido(int id)
    {
        var detalle = await _context.DetallesPedido.FindAsync(id);
        if (detalle == null)
        {
            return NotFound();
        }

        _context.DetallesPedido.Remove(detalle);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}

