using DataAccess.CRUD;
using DataAccess.DAO;
using DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BL
{
    public class UserManager
    {
        private readonly UserCrudFactory userCrudFactory;
        private readonly IEmailSender emailSender;
        private readonly ApplicationDbContext _context;
        private readonly SqlDao dao;
        private readonly ILogger<UserManager> _logger; // Agrega el logger

        // Actualiza el constructor para recibir el logger
        public UserManager(UserCrudFactory userCrudFactory, IEmailSender emailSender, ApplicationDbContext context, ILogger<UserManager> logger)
        {
            this.userCrudFactory = userCrudFactory;
            this.emailSender = emailSender;
            _context = context ?? throw new ArgumentNullException(nameof(context));
            dao = SqlDao.GetInstance();
            _logger = logger; // Inicializa el logger
        }

        public async Task CreateUser(User user)
        {
            var passwordHasher = new PasswordHasher<User>();
            user.Contrasena = passwordHasher.HashPassword(user, user.Contrasena);

            userCrudFactory.Create(user);

            var otp = new OTP
            {
                CorreoElectronico = user.CorreoElectronico,
                CodigoOTP = VerificationCodeGenerator.GenerateVerificationCode(),
                FechaCreacion = DateTime.UtcNow,
                Usado = false
            };

            userCrudFactory.CreateOTP(otp);

            var emailMessage = $"Tu código de verificación es: {otp.CodigoOTP}";
            await emailSender.SendEmailAsync(user.CorreoElectronico, "Código de Verificación", emailMessage);
        }

        public async Task<User> GetUserByCorreo(string Email)
        {
            return await Task.Run(() => userCrudFactory.RetrieveByEmail(Email));
        }

        public void UpdateUser(User user)
        {
            userCrudFactory.Update(user);
        }

        public void DeleteUser(string correo)
        {
            var user = userCrudFactory.RetrieveByEmail(correo);
            if (user != null)
            {
                userCrudFactory.Delete(user);
            }
        }

        public void CreateOTP(OTP otp)
        {
            userCrudFactory.CreateOTP(otp);
        }

        public OTP RetrieveOTP(string correo)
        {
            return userCrudFactory.RetrieveOTP(correo);
        }

        public OTP RetrieveLatestOTP(string correo)
        {
            return userCrudFactory.RetrieveLatestOTP(correo);
        }

        public void UpdateOTP(OTP otp)
        {
            userCrudFactory.UpdateOTP(otp);
        }

        public bool VerifyCode(string email, string code)
        {
            var otp = RetrieveLatestOTP(email);
            if (otp != null && otp.CodigoOTP.Equals(code, StringComparison.OrdinalIgnoreCase) && !otp.Usado)
            {
                otp.Usado = true;
                UpdateOTP(otp);

                var user = GetUserByCorreo(email).Result;
                if (user != null)
                {
                    user.CorreoVerificado = true;
                    UpdateUser(user);
                }

                return true;
            }
            return false;
        }

        public async Task ResendOTP(string email)
        {
            var otp = RetrieveLatestOTP(email);

            if (otp != null)
            {
                otp.CodigoOTP = VerificationCodeGenerator.GenerateVerificationCode();
                otp.FechaCreacion = DateTime.UtcNow;
                otp.Usado = false;
                UpdateOTP(otp);
            }
            else
            {
                otp = new OTP
                {
                    CorreoElectronico = email,
                    CodigoOTP = VerificationCodeGenerator.GenerateVerificationCode(),
                    FechaCreacion = DateTime.UtcNow,
                    Usado = false
                };

                CreateOTP(otp);
            }

            var emailMessage = $"Tu código de verificación es: {otp.CodigoOTP}";
            await emailSender.SendEmailAsync(email, "Código de Verificación", emailMessage);
        }

        public string GeneratePasswordResetToken(User user)
        {
            var token = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            return token;
        }

        public bool ResetPassword(string email, string token, string newPassword)
        {
            var user = userCrudFactory.RetrieveByEmail(email);
            if (user == null || !ValidateToken(user, token))
            {
                return false;
            }

            var passwordHasher = new PasswordHasher<User>();
            user.Contrasena = passwordHasher.HashPassword(user, newPassword);
            userCrudFactory.Update(user);

            return true;
        }

        private bool ValidateToken(User user, string token)
        {
            // Token validation logic here (if needed)
            return true;
        }

        public async Task<User> TestRetrieveUserByEmail(string correo)
        {
            var user = await Task.Run(() => userCrudFactory.RetrieveByEmail(correo));
            return user;
        }

        public List<User> GetAllEntrenadores()
        {
            return userCrudFactory.RetrieveAllEntrenadores();
        }

        public User GetUserRolAccess(string correo)
        {
            return userCrudFactory.RetrieveRolAccess(correo);
        }

        // Método asincrónico para obtener todos los entrenadores utilizando la base de datos directamente sin Entity Framework
        public async Task<List<User>> GetUsersByRoleAsync(string roleName)
        {
            var operation = new SqlOperation
            {
                ProcedureName = "GetUsersByRoleName"
            };

            operation.AddVarcharParam("RoleName", roleName);

            var result = await dao.ExecuteStoredProcedureWithQueryAsync(operation);

            var entrenadores = new List<User>();
            foreach (var row in result)
            {
                var user = new User
                {
                    CorreoElectronico = row["CorreoElectronico"].ToString(), // PK
                    Nombre = row["Nombre"].ToString(),
                    Contrasena = row["Contrasena"].ToString(),
                    Telefono = row["Telefono"].ToString(),
                    TipoUsuario = row["TipoUsuario"].ToString(),
                    FechaRegistro = Convert.ToDateTime(row["FechaRegistro"]),
                    CorreoVerificado = Convert.ToBoolean(row["CorreoVerificado"]),
                    TelefonoVerificado = Convert.ToBoolean(row["TelefonoVerificado"]),
                    Estado = Convert.ToBoolean(row["Estado"]),
                    HaPagado = Convert.ToBoolean(row["HaPagado"]),
                    RolAcceso = roleName // Asigna el nombre del rol directamente
                };
                entrenadores.Add(user);
            }

            return entrenadores;
        }

        public async Task<List<User>> SearchUsersByName(string name)
        {
            try
            {
                var operation = new SqlOperation
                {
                    ProcedureName = "sp_SearchUsersByName"
                };
                operation.AddVarcharParam("Name", "%" + name + "%");

                var result = await dao.ExecuteStoredProcedureWithQueryAsync(operation);

                var users = new List<User>();
                foreach (var row in result)
                {
                    var user = new User
                    {
                        CorreoElectronico = row["CorreoElectronico"].ToString(),
                        Nombre = row["Nombre"].ToString(),
                        Contrasena = row["Contrasena"].ToString(),
                        Telefono = row["Telefono"].ToString(),
                        TipoUsuario = row["TipoUsuario"].ToString(),
                        FechaRegistro = Convert.ToDateTime(row["FechaRegistro"]),
                        CorreoVerificado = Convert.ToBoolean(row["CorreoVerificado"]),
                        TelefonoVerificado = Convert.ToBoolean(row["TelefonoVerificado"]),
                        Estado = Convert.ToBoolean(row["Estado"]),
                        HaPagado = Convert.ToBoolean(row["HaPagado"]),
                        RolAcceso = row.ContainsKey("RolId") ? row["RolId"].ToString() : null // Usa "RolId" en lugar de "RolAcceso"
                    };
                    users.Add(user);
                }

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la búsqueda de usuarios por nombre");
                throw new Exception("Error al buscar usuarios por nombre: " + ex.Message);
            }
        }
    }
}
