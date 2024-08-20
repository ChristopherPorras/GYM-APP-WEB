using BL;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace API.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CitaController : ControllerBase
    {
        private readonly CitaManager _manager;
        private readonly UserManager _uManager;
        private readonly IEmailSender _emailSender;

        public CitaController(CitaManager manager, UserManager uManager, IEmailSender emailSender)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager));
            _uManager = uManager ?? throw new ArgumentNullException(nameof(uManager));
            _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CreateCita([FromBody] Cita cita)
        {
            if (cita == null)
            {
                return BadRequest("Cita no puede ser nula");
            }

            try
            {
                // Asegúrate de que cita.FechaCita tiene la fecha seleccionada por el usuario y no la fecha actual.
                var citaExistente = await _manager.GetCitaByDateAndUserAsync(cita.CorreoElectronico, cita.FechaCita);

                if (citaExistente != null)
                {
                    return Conflict(new { message = "Ya tienes una cita agendada para esa fecha y hora." });
                }

                // Guardar la cita en la base de datos tal como se recibe
                await _manager.CreateCitaAsync(cita);

                // Obtener información del usuario y del entrenador
                var usuario = await _uManager.GetUserByCorreo(cita.CorreoElectronico);
                var entrenador = await _uManager.GetUserByCorreo(cita.EntrenadorCorreo);

                if (usuario != null && entrenador != null)
                {
                    // Enviar correo electrónico de confirmación
                    var message = $"Hola {usuario.Nombre},\n\n" +
                                  $"Tu cita de medición corporal ha sido agendada exitosamente.\n\n" +
                                  $"Detalles de la cita:\n" +
                                  $"Fecha y Hora: {cita.FechaCita:dddd, dd MMMM yyyy hh:mm tt}\n" +
                                  $"Entrenador: {entrenador.Nombre}\n\n" +
                                  "¡Te esperamos!";
                    var subject = "Confirmación de Cita - Medición Corporal";

                    await _emailSender.SendEmailAsync(usuario.CorreoElectronico, subject, message);
                }

                return Ok(new { message = "Cita creada exitosamente." });
            }
            catch (Exception ex)
            {
                // Captura más detalles del error
                Console.WriteLine($"Error en CreateCita: {ex.Message} - {ex.StackTrace}");
                return StatusCode(500, new { message = "Ocurrió un error al crear la cita.", details = ex.Message, stackTrace = ex.StackTrace });
            }
        }

        [HttpGet("{correoElectronico}")]
        public async Task<ActionResult<List<Cita>>> GetCitaAgendada(string correoElectronico)
        {
            if (string.IsNullOrEmpty(correoElectronico))
            {
                return BadRequest("Correo electrónico no puede ser nulo o vacío.");
            }

            try
            {
                var citasAgendadas = await _manager.GetCitaAgendadaByUserAsync(correoElectronico);

                if (citasAgendadas == null || !citasAgendadas.Any())
                {
                    return Ok(new List<Cita>()); // Devuelve un array vacío si no hay citas
                }

                return Ok(citasAgendadas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetCitaAgendada: {ex.Message} - {ex.StackTrace}");
                return StatusCode(500, new { message = "Ocurrió un error al obtener la cita agendada.", details = ex.Message });
            }
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCita([FromBody] Cita cita)
        {
            if (cita == null || cita.ID <= 0)
            {
                return BadRequest("Datos de cita inválidos.");
            }

            try
            {
                await _manager.UpdateCitaAsync(cita);
                return Ok(new { message = "Cita actualizada exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al actualizar la cita.", details = ex.Message });
            }
        }

        [HttpDelete("{correoElectronico}/{fechaCita}")]
        public async Task<ActionResult> DeleteCita(string correoElectronico, DateTime fechaCita)
        {
            if (string.IsNullOrEmpty(correoElectronico))
            {
                return BadRequest("Correo electrónico no puede ser nulo o vacío.");
            }

            try
            {
                await _manager.DeleteCitaAsync(correoElectronico, fechaCita);
                return Ok(new { message = "Cita eliminada exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al eliminar la cita.", details = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetDisponibilidad()
        {
            try
            {
                var citas = await _manager.GetAllCitasAsync();
                var disponibilidad = GetHorariosDisponibles(citas);
                return Ok(disponibilidad);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocurrió un error al obtener la disponibilidad.", details = ex.Message });
            }
        }

        private Dictionary<string, Dictionary<string, bool>> GetHorariosDisponibles(IEnumerable<Cita> citas)
        {
            var horarios = new[] { "6 am - 8 am", "10 am - 12 pm", "5 pm - 7 pm", "7 pm - 9 pm" };
            var dias = new[] { "Lunes", "Martes", "Miércoles", "Jueves", "Viernes", "Sábado", "Domingo" };

            var disponibilidad = new Dictionary<string, Dictionary<string, bool>>();

            foreach (var dia in dias)
            {
                var horariosDisponibles = horarios.ToDictionary(h => h, h => true);

                foreach (var cita in citas)
                {
                    if (cita.FechaCita == null) continue;

                    var citaDia = GetNombreDiaSemana(cita.FechaCita.DayOfWeek);
                    if (dia.Equals(citaDia, StringComparison.OrdinalIgnoreCase))
                    {
                        var hora = GetHorario(cita.FechaCita);
                        if (hora != null && horariosDisponibles.ContainsKey(hora))
                        {
                            horariosDisponibles[hora] = false;
                        }
                    }
                }

                disponibilidad[dia] = horariosDisponibles;
            }

            return disponibilidad;
        }

        private string GetHorario(DateTime FechaCita)
        {
            if (FechaCita.Hour >= 6 && FechaCita.Hour < 8) return "6 am - 8 am";
            if (FechaCita.Hour >= 10 && FechaCita.Hour < 12) return "10 am - 12 pm";
            if (FechaCita.Hour >= 17 && FechaCita.Hour < 19) return "5 pm - 7 pm";
            if (FechaCita.Hour >= 19 && FechaCita.Hour < 21) return "7 pm - 9 pm";
            return null;
        }

        private string GetNombreDiaSemana(DayOfWeek dia)
        {
            return dia switch
            {
                DayOfWeek.Monday => "Lunes",
                DayOfWeek.Tuesday => "Martes",
                DayOfWeek.Wednesday => "Miércoles",
                DayOfWeek.Thursday => "Jueves",
                DayOfWeek.Friday => "Viernes",
                DayOfWeek.Saturday => "Sábado",
                DayOfWeek.Sunday => "Domingo",
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllEntrenadores()
        {
            try
            {
                var entrenadores = await _uManager.GetUsersByRoleAsync("Entrenador");

                var entrenadoresFiltrados = entrenadores.Select(e => new
                {
                    CorreoElectronico = e.CorreoElectronico,
                    Nombre = e.Nombre
                }).ToList();

                return Ok(entrenadoresFiltrados);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error en GetAllEntrenadores: {ex.Message} - {ex.StackTrace}");
                return StatusCode(500, new { message = "Ocurrió un error al obtener los entrenadores.", details = ex.Message });
            }
        }
    }
}
