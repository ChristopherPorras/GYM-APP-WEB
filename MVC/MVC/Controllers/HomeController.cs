using Microsoft.AspNetCore.Mvc;
using DTO; // Para usar ResetPasswordViewModel
using BL;  // Para usar UserManager
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager _manager;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public HomeController(ILogger<HomeController> logger, UserManager manager, IEmailSender emailSender, IConfiguration configuration)
        {
            _logger = logger;
            _manager = manager;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LogPage(string userName)
        {
            ViewData["UserName"] = userName;
            return View();
        }

        public IActionResult Verification()
        {
            return View();
        }

        public IActionResult Rutinas()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PasswordReset()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendPasswordResetLink(string email)
        {
            _logger.LogInformation($"Intentando enviar enlace de restablecimiento de contraseña a {email}");

            var user = await _manager.GetUserByCorreo(email);
            if (user == null)
            {
                _logger.LogWarning($"No se encontró ningún usuario con el correo: {email}");
                return BadRequest(new { message = "El correo electrónico no está registrado." });
            }

            var token = _manager.GeneratePasswordResetToken(user);
            _logger.LogInformation($"Token generado para el usuario {email}: {token}");

            var baseUrl = _configuration["MvcAppBaseUrl"];
            var resetLink = $"{baseUrl}/Home/ResetPassword?email={email}&token={token}";

            var emailDto = new EmailDto
            {
                ToEmail = email,
                Subject = "Restablecimiento de contraseña",
                Message = $"Para restablecer tu contraseña, haz clic en el siguiente enlace: <a href=\"{resetLink}\">Restablecer contraseña</a>"
            };

            _logger.LogInformation($"Enviando correo de restablecimiento de contraseña a {email}");
            await _emailSender.SendEmailAsync(emailDto.ToEmail, emailDto.Subject, emailDto.Message);

            _logger.LogInformation($"Correo enviado exitosamente a {email}");
            return Ok(new { message = "Se ha enviado un enlace de restablecimiento de contraseña a tu correo electrónico." });
        }



        [HttpGet]
        public IActionResult ResetPassword(string email, string token)
        {
            var model = new ResetPasswordViewModel { Email = email, Token = token };
            return View(model);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = _manager.ResetPassword(model.Email, model.Token, model.NewPassword);
            if (result)
            {
                return RedirectToAction("LogPage", new { message = "Contraseña restablecida con éxito. Ahora puedes iniciar sesión." });
            }

            ModelState.AddModelError(string.Empty, "El restablecimiento de contraseña ha fallado.");
            return View(model);
        }

        public IActionResult Equipos()
        {
            return View();
        }

        public IActionResult UsuarioRole()
        {
            return View();
        }

        public IActionResult ConfigCitas()
        {
            return View();
        }

        public IActionResult Entrenador()
        {
            return View();
        }

        public IActionResult Ejercicios()
        {
            return View();
        }
        public IActionResult SesionUsuario()
        {
            return View();
        }
        public IActionResult SesionRecep()
        {
            return View();
        }
        public IActionResult SesionEntre()
        {
            return View();
        }
        public IActionResult SesionAdmin()
        {
            return View();
        }
        public IActionResult PayPage()
        {
            return View();
        }

        public IActionResult Contactenos()
        {
            return View();
        }
        public IActionResult Progreso()

        {
            return View();
        }
        public IActionResult Team()

        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new DTO.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
