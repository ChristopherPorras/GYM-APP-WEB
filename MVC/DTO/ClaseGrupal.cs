using System;

namespace DTO
{
    public class ClaseGrupal : BaseClass
    {
        public int ClaseGrupalID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int Cupo { get; set; }
        public DateTime Horario { get; set; }
        public string InstructorCorreo { get; set; }
        public string UsuarioCorreo { get; set; }
        public string EntrenadorCorreo { get; set; }  
    }
}
