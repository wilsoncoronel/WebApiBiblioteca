﻿using System.ComponentModel.DataAnnotations;

namespace HolaMundoWebAPI.DTOs
{
    public class LibroCreacionDTO
    {
        [Required]
        [StringLength(250, ErrorMessage ="El campo {0} debe tener {1} carácteres o menos.")]
        public required string Titulo { get; set; }
        public List<int> AutoresIds { get; set; } = [];
    }
}
