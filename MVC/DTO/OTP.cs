namespace DTO
{
    public class OTP : BaseClass
    {
        public int ID { get; set; } // This should be included in the OTP class
        public string CorreoElectronico { get; set; }
        public string CodigoOTP { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Usado { get; set; }
    }
}
