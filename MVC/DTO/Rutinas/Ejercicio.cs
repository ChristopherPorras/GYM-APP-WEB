using System.Text.Json.Serialization;

namespace DTO.Rutinas
{
    public class Ejercicio : BaseClass
    {
        public int EjercicioId { get; set; }

        public string Nombre { get; set; }

        public string Tipo { get; set; }

        public string Descripcion { get; set; }

        public decimal? Peso { get; set; }

        public int? Tiempo { get; set; }

        public bool? Amrap { get; set; }
    }
}
