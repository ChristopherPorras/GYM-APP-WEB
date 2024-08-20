using System;

namespace DTO
{
    public class RutinaDTO : BaseClass
    {
        public string CorreoElectronico { get; set; } 
        public int MedicionId { get; set; }
        public string EntrenadorCorreo { get; set; }  
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }
}
