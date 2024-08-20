using System;

namespace DTO.Rutinas
{
    public class ProgresoUsuario : BaseClass
    {
        public int Id { get; set; }
        public string CorreoElectronico { get; set; }
        public DateTime FechaProgreso { get; set; }
        public decimal Peso { get; set; }
        public decimal MasaMuscular { get; set; }
        public decimal PorcentajeGrasa { get; set; }
    }
}
