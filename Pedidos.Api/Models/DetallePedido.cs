using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pedidos.Api.Models;

[Table("DetallePedido")]
public class DetallePedido
{
    [Key]
    public int IdDetalle { get; set; }

    [Required]
    public int IdPedido { get; set; }

    [Required]
    public int IdProducto { get; set; }

    [Required]
    public int Cantidad { get; set; }

    [Column("PrecioUnitario", TypeName = "decimal(10,2)")]
    public decimal PrecioUnitario { get; set; }
}

