using System;

namespace DTO
{
    public class CreateUserRequest
    {
        public string Nombre { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public string Contrasena { get; set; }
        public string TipoUsuario { get; set; } = "Cliente";
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public bool CorreoVerificado { get; set; } = false;
        public bool TelefonoVerificado { get; set; } = false;
        public bool Estado { get; set; } = true;
        public bool HaPagado { get; set; } = false;
    }
}
