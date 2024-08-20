using System;

namespace DTO
{
    public class ProgresoUsuarioDTO : BaseClass
    {
        public string CorreoElectronico { get; set; }
        public DateTime FechaProgreso { get; set; }
        public decimal Peso { get; set; }
        public decimal MasaMuscular { get; set; }
        public decimal PorcentajeGrasa { get; set; }
    }
}
