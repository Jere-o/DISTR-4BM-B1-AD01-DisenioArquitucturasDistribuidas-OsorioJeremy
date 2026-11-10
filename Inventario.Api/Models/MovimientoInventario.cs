using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario.Api.Models;

[Table("MovimientoInventario")]
public class MovimientoInventario
{
    [Key]
    public int IdMovimiento { get; set; }

    [Required]
    public int IdProducto { get; set; }

    [Required]
    public int IdBodega { get; set; }

    [Required]
    [StringLength(50)]
    public string Tipo { get; set; } = string.Empty;

    public int Cantidad { get; set; }

    public DateTime Fecha { get; set; }
}

