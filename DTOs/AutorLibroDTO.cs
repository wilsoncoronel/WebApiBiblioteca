using HolaMundoWebAPI.Entidades;

namespace HolaMundoWebAPI.DTOs
{
    public class AutorLibroDTO
    {
        public int AutorId { get; set; }
        public int LibroId { get; set; }
        public int Orden { get; set; }
        public AutorDTO Autor { get; set; }
        public LibroDTO Libro { get; set; }


    }
}
