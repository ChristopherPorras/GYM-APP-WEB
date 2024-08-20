namespace DTO
{
    public class UsuarioRole : BaseClass
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
        public int RolId { get; set; }
    }
}
