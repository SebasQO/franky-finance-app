using System.ComponentModel.DataAnnotations;

namespace FrankyFinance.Models
{
    // ViewModel para manejar la información del formulario de inicio de sesión
    public class LoginViewModel
    {
        // Correo electrónico del usuario (obligatorio y con formato válido)
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        // Contraseña del usuario (obligatoria)
        [Required(ErrorMessage = "Password is required.")]
        public string? Password { get; set; }
    }
}
