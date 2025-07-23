using HolaMundoWebAPI.Entidades;
using System.ComponentModel.DataAnnotations;

namespace HolaMundoWebAPI.DTOs
{
    public class LibroDTO
    {
        public int Id { get; set; }
        public required string Titulo { get; set; }
        public List<AutorLibroDTO> AutorLibroDto { get; set; }
    }
}
