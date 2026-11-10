using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Catalogo.Api.Models;

[Table("Marca")]
public class Marca
{
    [Key]
    public int IdMarca { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string PaisOrigen { get; set; } = string.Empty;
}
