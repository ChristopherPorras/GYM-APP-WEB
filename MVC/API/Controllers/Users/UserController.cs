using Microsoft.AspNetCore.Mvc;
using BL;
using DTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers.Users
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager _manager;
        private readonly ILogger<UserController> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _configuration;

        public UserController(UserManager manager, ILogger<UserController> logger, IEmailSender emailSender, IConfiguration configuration)
        {
            _manager = manager;
            _logger = logger;
            _emailSender = emailSender;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the user.");
                return BadRequest(ModelState);
            }

            try
            {
                var existingUser = await _manager.GetUserByCorreo(request.CorreoElectronico);
                if (existingUser != null)
                {
                    return Conflict(new { message = "El correo electrónico ya está registrado." });
                }

                var user = new User
                {
                    Nombre = request.Nombre,
                    CorreoElectronico = request.CorreoElectronico,
                    Contrasena = request.Contrasena,
                    Telefono = request.Telefono,
                    TipoUsuario = request.TipoUsuario,
                    FechaRegistro = DateTime.UtcNow,
                    CorreoVerificado = false,
                    TelefonoVerificado = request.TelefonoVerificado,
                    Estado = request.Estado,
                    HaPagado = request.HaPagado
                };

                await _manager.CreateUser(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyCode([FromBody] VerificationRequest request)
        {
            _logger.LogInformation("Verificando código OTP para el correo: " + request.Email);
            _logger.LogInformation("Código ingresado: " + request.Code);

            var otp = _manager.RetrieveLatestOTP(request.Email);

            if (otp != null)
            {
                _logger.LogInformation("Código almacenado en la base de datos: " + otp.CodigoOTP);

                if (otp.CodigoOTP.Equals(request.Code, StringComparison.OrdinalIgnoreCase) && !otp.Usado)
                {
                    _logger.LogInformation("El código OTP coincide y no ha sido usado.");

                    otp.Usado = true;
                    _manager.UpdateOTP(otp);

                    var user = await _manager.GetUserByCorreo(request.Email);
                    if (user != null)
                    {
                        user.CorreoVerificado = true;
                        _manager.UpdateUser(user);
                    }

                    return Ok(new { isValid = true, userName = user.Nombre, redirectUrl = Url.Action("LogPage", "Home", new { userName = user.Nombre }) });
                }
                else
                {
                    _logger.LogWarning("El código OTP no coincide o ya ha sido usado.");
                    return Unauthorized(new { isValid = false, message = "Código incorrecto. Inténtalo nuevamente." });
                }
            }
            else
            {
                _logger.LogWarning("No se encontró un OTP para este correo.");
                return Unauthorized(new { isValid = false, message = "Código incorrecto. Inténtalo nuevamente." });
            }
        }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Login attempt for user: " + request.Email);
            try
            {
                var user = await _manager.GetUserByCorreo(request.Email);
                if (user == null)
                {
                    return Unauthorized(new { message = "Correo electrónico o contraseña incorrectos. Inténtalo nuevamente." });
                }

                var passwordHasher = new PasswordHasher<User>();
                var result = passwordHasher.VerifyHashedPassword(user, user.Contrasena, request.Password);
                if (result != PasswordVerificationResult.Success)
                {
                    return Unauthorized(new { message = "Correo electrónico o contraseña incorrectos. Inténtalo nuevamente." });
                }

                if (!user.CorreoVerificado)
                {
                    await _manager.ResendOTP(user.CorreoElectronico);
                    return Unauthorized(new { message = "Correo no verificado. Se ha enviado un nuevo código de verificación a tu correo electrónico.", requireVerification = true });
                }

                HttpContext.Session.SetString("UserEmail", user.CorreoElectronico);
                HttpContext.Session.SetString("UserName", user.Nombre);
                HttpContext.Session.SetString("UserRole", user.TipoUsuario);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.CorreoElectronico),
                    new Claim(ClaimTypes.Role, user.TipoUsuario)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return Ok(new
                {
                    success = true,
                    userAutenticado = user
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckEmailExists([FromBody] EmailRequest request)
        {
            try
            {
                var user = await _manager.GetUserByCorreo(request.EmailRecipient);
                if (user != null)
                {
                    return Ok(new { exists = true, message = "El correo electrónico ya está registrado." });
                }
                return Ok(new { exists = false });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking email existence");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword([FromBody] EmailRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.EmailRecipient))
            {
                return BadRequest(new { message = "El correo electrónico es obligatorio." });
            }

            var user = await _manager.GetUserByCorreo(request.EmailRecipient);
            if (user == null)
            {
                return BadRequest(new { message = "El correo electrónico no está registrado." });
            }

            if (!user.CorreoVerificado)
            {
                await _manager.ResendOTP(user.CorreoElectronico);
                return Ok(new
                {
                    message = "Correo no verificado. Se ha enviado un nuevo código de verificación a tu correo electrónico.",
                    requireVerification = true
                });
            }

            var token = _manager.GeneratePasswordResetToken(user);
            var mvcBaseUrl = _configuration["MvcAppBaseUrl"];
            var resetLink = $"{mvcBaseUrl}/Home/ResetPassword?email={request.EmailRecipient}&token={token}";

            var emailDto = new EmailDto
            {
                ToEmail = request.EmailRecipient,
                Subject = "Restablecimiento de contraseña",
                Message = $"Para restablecer tu contraseña, haz clic en el siguiente enlace: <a href=\"{resetLink}\">Restablecer contraseña</a>"
            };

            await _emailSender.SendEmailAsync(emailDto.ToEmail, emailDto.Subject, emailDto.Message);

            return Ok(new { message = "Se ha enviado un enlace de restablecimiento de contraseña a tu correo electrónico." });
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return Ok(new { message = "Sesión cerrada exitosamente." });
        }

        [HttpGet]
        public int GetUserRol(string email)
        {
            int rol = 0;
            var user = _manager.GetUserRolAccess(email);
            if (user != null)
            {
                if (int.TryParse(user.RolAcceso, out int result))
                {
                    rol = result;
                }
            }
            return rol;
        }

        [HttpGet]
        public ActionResult<string> GetUserEmail()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return NotFound(new { message = "Email not found" });
            }
            return Ok(new { email = userEmail });
        }


        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<string>> GetUserNameByEmail(string email)
        {
            var user = await _manager.GetUserByCorreo(email);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }
            return Ok(new { nombre = user.Nombre });
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> SearchByName(string name)
        {
            try
            {
                // Llama al método del manager que busca los usuarios por nombre
                var users = await _manager.SearchUsersByName(name);
                if (users == null || users.Count == 0)
                {
                    return NotFound(new { message = "No users found" });
                }
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching users by name");
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
