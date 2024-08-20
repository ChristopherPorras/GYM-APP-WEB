using System.Text.Json.Serialization;

namespace DTO
{
    public class Descuentos : BaseClass
    {

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("porcentaje")]
        public decimal Porcentaje { get; set; }

        [JsonPropertyName("fechaInicio")]
        public DateTime FechaInicio { get; set; }

        [JsonPropertyName("fechaFin")]
        public DateTime FechaFin { get; set; }
    }
}
