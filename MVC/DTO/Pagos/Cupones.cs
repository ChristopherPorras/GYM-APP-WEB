using System;

namespace DTO
{
    public class Cupones : BaseClass
    {
        public int Id { get; set; }
        public string Codigo { get; set; }
        public int DescuentoId { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Usado { get; set; }

        public virtual Descuentos Descuento { get; set; }
    }
}
