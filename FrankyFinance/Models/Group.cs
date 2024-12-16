using System.ComponentModel.DataAnnotations;

namespace FrankyFinance.Models
{
    // Modelo que representa un grupo en la aplicación
    public class Group
    {
        // Identificador único del grupo (clave primaria)
        [Key]
        public int Id { get; set; }

        // Nombre del grupo (obligatorio)
        [Required]
        public string Name { get; set; }

        // Descripción opcional del grupo
        public string Description { get; set; }

        // Colección de gastos asociados a este grupo
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();

        // Colección de usuarios que pertenecen al grupo
        public ICollection<GroupUser> GroupUsers { get; set; } = new List<GroupUser>();

        // Colección de pagos asociados al grupo
        public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
    }
}
