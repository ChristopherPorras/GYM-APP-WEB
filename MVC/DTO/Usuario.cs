using System;

namespace DTO
{
    public class Usuario : BaseClass
    {
        public string CorreoElectronico { get; set; }
        public string Nombre { get; set; }
        public string Contrasena { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string Telefono { get; set; }
        public string TipoUsuario { get; set; }
        public bool Estado { get; set; }
        public bool HaPagado { get; set; }
        public bool CorreoVerificado { get; set; }
        public bool TelefonoVerificado { get; set; }
    }
}