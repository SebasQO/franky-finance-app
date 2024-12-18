using System.ComponentModel.DataAnnotations;

namespace FrankyFinance.Models
{
    // Modelo que representa la relación muchos a muchos entre Group y User
    public class GroupUser
    {
        // Clave foránea: Identificador del grupo
        public int GroupId { get; set; }

        // Propiedad de navegación: referencia al grupo asociado
        public Group Group { get; set; }

        // Clave foránea: Identificador del usuario
        public int UserId { get; set; }

        // Propiedad de navegación: referencia al usuario asociado
        public User User { get; set; }

        public string Role { get; set; }
    }
}
