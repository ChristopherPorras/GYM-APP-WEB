using BL;
using DTO;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RutinaController : ControllerBase
    {
        private readonly RutinaManager _manager;
        private readonly IEmailSender _emailSender;

        public RutinaController(RutinaManager manager, IEmailSender emailSender)
        {
            _manager = manager;
            _emailSender = emailSender;
        }

        [HttpPost("CreateRutina")]
        public IActionResult CreateRutina([FromBody] RutinaDTO rutina)
        {
            try
            {
                if (string.IsNullOrEmpty(rutina.CorreoElectronico))
                {
                    return BadRequest("Correo electrónico del cliente es obligatorio.");
                }

                if (string.IsNullOrEmpty(rutina.EntrenadorCorreo))
                {
                    return BadRequest("Correo electrónico del entrenador es obligatorio.");
                }

                _manager.Create(rutina);
                return Ok("Rutina creada exitosamente");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al crear la rutina: {ex.Message}");
            }
        }

        [HttpGet("GetRutina/{id}")]
        public IActionResult GetRutina(int id)
        {
            var rutina = _manager.RetrieveById<RutinaDTO>(id);
            return Ok(rutina);
        }

        [HttpGet("GetAllRutinas")]
        public IActionResult GetAllRutinas()
        {
            var rutinas = _manager.RetrieveAll<RutinaDTO>();
            return Ok(rutinas);
        }

        [HttpPut("UpdateRutina")]
        public IActionResult UpdateRutina([FromBody] RutinaDTO rutina)
        {
            _manager.Update(rutina);
            return Ok("Rutina actualizada exitosamente");
        }

        [HttpDelete("DeleteRutina/{id}")]
        public IActionResult DeleteRutina(int id)
        {
            _manager.Delete(new RutinaDTO { ID = id });
            return Ok("Rutina eliminada exitosamente");
        }

        [HttpPost("SendRutinaPdf")]
        public async Task<IActionResult> SendRutinaPdf([FromBody] EmailDto emailDto)
        {
            try
            {
                if (string.IsNullOrEmpty(emailDto.ToEmail))
                {
                    return BadRequest("El destinatario del correo es obligatorio.");
                }

                // Crear un mensaje personalizado
                var message = new StringBuilder();
                message.AppendLine($"Estimado/a {emailDto.ToEmail},");
                message.AppendLine();
                message.AppendLine($"Le informamos que el entrenador {emailDto.Subject} le ha asignado una nueva rutina.");
                message.AppendLine();
                message.AppendLine("Adjunto encontrará la rutina asignada.");
                message.AppendLine();
                message.AppendLine("¡Esperamos que disfrute su entrenamiento!");
                message.AppendLine();
                message.AppendLine("Saludos,");
                message.AppendLine("Su equipo de acondicionamiento físico");

                // Convertir el PDF base64 en un archivo adjunto
                byte[] pdfBytes = Convert.FromBase64String(emailDto.Message);
                MemoryStream pdfStream = new MemoryStream(pdfBytes);
                Attachment pdfAttachment = new Attachment(pdfStream, "rutina.pdf", MediaTypeNames.Application.Pdf);

                // Usar SmtpClient y MailMessage para enviar el correo con el archivo adjunto
                using (var mail = new MailMessage())
                {
                    mail.From = new MailAddress("your-email@example.com"); // Cambia esto por tu correo electrónico
                    mail.To.Add(emailDto.ToEmail);
                    mail.Subject = "Su nueva rutina de ejercicios";
                    mail.Body = message.ToString();
                    mail.Attachments.Add(pdfAttachment);

                    using (var smtp = new SmtpClient("smtp.example.com")) // Cambia esto por tu servidor SMTP
                    {
                        smtp.Port = 587; // Cambia esto por el puerto SMTP
                        smtp.Credentials = new System.Net.NetworkCredential("your-email@example.com", "your-password"); // Cambia esto por tus credenciales
                        smtp.EnableSsl = true;

                        await smtp.SendMailAsync(mail);
                    }
                }

                return Ok("PDF enviado correctamente");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error al enviar el PDF: {ex.Message}");
            }
        }
    }
}
