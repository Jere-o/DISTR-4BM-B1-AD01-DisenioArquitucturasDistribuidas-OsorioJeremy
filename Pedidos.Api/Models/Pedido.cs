using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pedidos.Api.Models;

[Table("Pedido")]
public class Pedido
{
    [Key]
    public int IdPedido { get; set; }

    [Required]
    public int IdCliente { get; set; }

    public DateTime Fecha { get; set; }

    [Column("Total", TypeName = "decimal(10,2)")]
    public decimal Total { get; set; }

    [Required]
    [StringLength(50)]
    public string Estado { get; set; } = string.Empty;
}

