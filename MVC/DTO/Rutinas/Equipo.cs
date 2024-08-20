using System.Text.Json.Serialization;

namespace DTO.Rutinas
{
    public class Equipo : BaseClass
    {
        [JsonPropertyName("equipoId")]
        public int EquipoId { get; set; }

        [JsonPropertyName("nombre")]
        public string Nombre { get; set; }

        [JsonPropertyName("descripcion")]
        public string Descripcion { get; set; }

        [JsonPropertyName("grupoMuscular")]
        public string GrupoMuscular { get; set; }

        [JsonPropertyName("cantidad")]
        public int Cantidad { get; set; }

        [JsonPropertyName("disponibilidad")]
        public bool Disponibilidad { get; set; }
    }
}
