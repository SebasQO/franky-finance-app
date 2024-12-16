using System.ComponentModel.DataAnnotations;

namespace FrankyFinance.Models
{
    // ViewModel para manejar los datos del formulario de registro de usuario
    public class RegisterViewModel
    {
        // Nombre del usuario (obligatorio)
        [Required(ErrorMessage = "Name is required.")]
        public string? Name { get; set; }

        // Correo electrónico del usuario (obligatorio y con validación de formato)
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }

        // Contraseña del usuario (obligatoria, con longitud mínima)
        [Required(ErrorMessage = "Password is required.")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string? Password { get; set; }

        // Confirmación de la contraseña (obligatoria, debe coincidir con la contraseña)
        [Required(ErrorMessage = "Please confirm your password.")]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
