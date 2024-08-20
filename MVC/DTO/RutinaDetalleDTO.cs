using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class RutinaDetalleDTO : BaseClass
    {
        public int EjercicioId { get; set; }
        public int Sets { get; set; }
        public int Repeticiones { get; set; }
        public decimal Peso { get; set; }
        public string DiaSemana { get; set; }
    }
}
