namespace DTO
{
    public class Pagos : BaseClass
    {
        public string CorreoElectronico { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; }
        public string EstadoPago { get; set; }
        public string TransactionId { get; set; } // Nuevo campo para el ID de transacción
    }
}
