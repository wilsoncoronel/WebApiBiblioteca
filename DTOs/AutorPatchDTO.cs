﻿using HolaMundoWebAPI.Validaciones;
using System.ComponentModel.DataAnnotations;

namespace HolaMundoWebAPI.DTOs
{
    public class AutorPatchDTO
    {
        [Required(ErrorMessage = "El campo {0} es requerido!!")]
        [StringLength(150, ErrorMessage = "El campo {0} debe tener {1} carácteres o menos")]
        [PrimeraLetraMayuscula]
        public required string Nombres { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido!!")]
        [StringLength(150, ErrorMessage = "El campo {0} debe tener {1} carácteres o menos")]
        [PrimeraLetraMayuscula]
        public required string Apellidos { get; set; }
        [StringLength(20, ErrorMessage = "El campo {0} debe tener {1} carácteres o menos")]
        public string? Identificacion { get; set; }
    }
}
