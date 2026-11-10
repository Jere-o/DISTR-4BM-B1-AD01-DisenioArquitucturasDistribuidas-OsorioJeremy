using Microsoft.EntityFrameworkCore;
using Catalogo.Api.Models;

namespace Catalogo.Api.Data;

public class CatalogoDBContext : DbContext
{
    public CatalogoDBContext(DbContextOptions<CatalogoDBContext> options) : base(options)
    {
    }

    public DbSet<Categoria> Categorias { get; set; }
    public DbSet<Marca> Marcas { get; set; }
    public DbSet<Producto> Productos { get; set; }
}
