using System;

namespace DTO
{
    public class Cita : BaseClass
    {
        // IDs del usuario y del entrenador (relaciones de clave foránea)
        public string CorreoElectronico { get; set; }
        public string EntrenadorCorreo { get; set; }

        // Fecha y hora de la cita
        public DateTime FechaCita { get; set; }

        // Estado de la cita
        public string Estado { get; set; }
    }
}
