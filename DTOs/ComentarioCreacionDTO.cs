using System.ComponentModel.DataAnnotations;

namespace HolaMundoWebAPI.DTOs
{
    public class ComentarioCreacionDTO
    {
        [Required]
        public string Cuerpo { get; set; }
    }
}
