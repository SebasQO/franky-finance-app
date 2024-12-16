using System.ComponentModel.DataAnnotations;

namespace FrankyFinance.Models
{
    // Modelo que representa a un usuario en la aplicación
    public class User
    {
        // Identificador único del usuario (clave primaria)
        [Key]
        public int Id { get; set; }

        // Nombre del usuario (obligatorio)
        [Required]
        public string? Name { get; set; }

        // Correo electrónico del usuario (obligatorio, con formato válido)
        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        // Contraseña del usuario (obligatoria)
        [Required]
        public string? Password { get; set; }

        // Lista de relaciones del usuario con los grupos (muchos a muchos)
        public ICollection<GroupUser> GroupUsers { get; set; } = new List<GroupUser>();
    }
}
