using System.ComponentModel.DataAnnotations;

namespace DTO
{
    public class ResetPasswordViewModel
    {
        public string Email { get; set; }
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Las contraseñas no coinciden.")]
        public string ConfirmNewPassword { get; set; }
    }
}
