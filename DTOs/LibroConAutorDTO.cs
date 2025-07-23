using HolaMundoWebAPI.Entidades;
using System.ComponentModel.DataAnnotations;

namespace HolaMundoWebAPI.DTOs
{
    public class LibroConAutorDTO: LibroDTO
    {   
        public AutorDTO AutorDTO { get; set; }
    }
}