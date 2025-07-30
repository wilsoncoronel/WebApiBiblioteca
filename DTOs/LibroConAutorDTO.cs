using HolaMundoWebAPI.Entidades;
using System.ComponentModel.DataAnnotations;

namespace HolaMundoWebAPI.DTOs
{
    public class LibroConAutorDTO: LibroDTO
    {
        public List<AutorDTO> Autores { get; set; } = [];
    }
}