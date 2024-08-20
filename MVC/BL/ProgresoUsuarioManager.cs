using DataAccess.CRUD;
using DTO;
using System.Collections.Generic;

namespace BL
{
    public class ProgresoUsuarioManager
    {
        private readonly ProgresoUsuarioCrudFactory progresoCrudFactory;

        public ProgresoUsuarioManager()
        {
            progresoCrudFactory = new ProgresoUsuarioCrudFactory();
        }

        public List<ProgresoUsuarioDTO> ObtenerProgresoPorCorreo(string correo)
        {
            var progreso = progresoCrudFactory.RetrieveByCorreo(correo);

            // Si no hay registros, generamos un registro de ejemplo
            if (progreso == null || progreso.Count == 0)
            {
                var ejemplo = new ProgresoUsuarioDTO
                {
                    CorreoElectronico = correo,
                    FechaProgreso = DateTime.Now,
                    Peso = 70,  // Ejemplo de peso inicial
                    MasaMuscular = 30,  // Ejemplo de masa muscular
                    PorcentajeGrasa = 20  // Ejemplo de porcentaje de grasa
                };

                progresoCrudFactory.Create(ejemplo);
                progreso.Add(ejemplo);  // Añadir el ejemplo a la lista de progreso
            }

            return progreso;
        }

        public void CrearProgreso(ProgresoUsuarioDTO progreso)
        {
            progresoCrudFactory.Create(progreso);
        }

        public void ActualizarProgreso(ProgresoUsuarioDTO progreso)
        {
            progresoCrudFactory.Update(progreso);
        }

        public void EliminarProgreso(int id)
        {
            var progreso = progresoCrudFactory.RetrieveById(id);
            if (progreso != null)
            {
                progresoCrudFactory.Delete(progreso);
            }
        }
    }
}
