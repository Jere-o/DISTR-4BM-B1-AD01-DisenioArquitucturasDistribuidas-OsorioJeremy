using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Inventario.Api.Models;

[Table("Bodega")]
public class Bodega
{
    [Key]
    public int IdBodega { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Ciudad { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Direccion { get; set; } = string.Empty;
}

