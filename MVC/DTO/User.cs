using System;
using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class User : BaseClass
    {
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        public string CorreoElectronico { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Contrasena { get; set; }

        [Required]
        [Phone]
        public string Telefono { get; set; }

        [Required]
        public string TipoUsuario { get; set; } = "Usuario";

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool CorreoVerificado { get; set; } = false;
        public bool TelefonoVerificado { get; set; } = false;
        public bool Estado { get; set; } = true;
        public bool HaPagado { get; set; } = false;

        public string? RolAcceso { get; set; }
    }
}
