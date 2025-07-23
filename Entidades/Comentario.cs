﻿using System.ComponentModel.DataAnnotations;

namespace HolaMundoWebAPI.Entidades
{
    public class Comentario
    {
        public Guid Id { get; set; }
        [Required]
        public  string Cuerpo { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public int LibroId { get; set; }
        public Libro? Libro { get; set; }
    }
}
