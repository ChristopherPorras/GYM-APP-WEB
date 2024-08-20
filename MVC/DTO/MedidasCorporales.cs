using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class MedidasCorporales : BaseClass
    {
        public int MedidasId { get; set; }
        public string CorreoElectronico { get; set; } 
        public DateTime FechaMedicion { get; set; }
        public decimal Peso { get; set; }
        public decimal Altura { get; set; }
        public string EntrenadorCorreo { get; set; }
    }

}
