using System.ComponentModel.DataAnnotations;

namespace HolaMundoWebAPI.DTOs
{
    public class ComentarioDTO
    {
        public Guid Id { get; set; }
        public string Cuerpo { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}
