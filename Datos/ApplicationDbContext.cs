using HolaMundoWebAPI.Entidades;
using Microsoft.EntityFrameworkCore;
namespace HolaMundoWebAPI.Datos
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Autor> Autores { get; set; }//crea una tabla autores a partir de la clase autor
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }

        public DbSet<AutorLibro> AutoresLibros { get; set; }
    }
}