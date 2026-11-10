using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario.Api.Models;

[Table("ReservaStock")]
public class ReservaStock
{
    [Key]
    public int IdReserva { get; set; }

    [Required]
    public int IdProducto { get; set; }

    [Required]
    public int IdBodega { get; set; }

    public int Cantidad { get; set; }

    [Required]
    [StringLength(50)]
    public string Estado { get; set; } = string.Empty;
}

