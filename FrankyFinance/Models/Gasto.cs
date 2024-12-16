using System.ComponentModel.DataAnnotations;

namespace FrankyFinance.Models
{
    // Modelo que representa un gasto asociado a un grupo
    public class Gasto
    {
        // Identificador único del gasto (clave primaria)
        [Key]
        public int Id { get; set; }

        // Descripción del gasto (obligatoria)
        [Required(ErrorMessage = "The description is required.")]
        public string Description { get; set; }

        // Monto total del gasto (obligatorio)
        [Required(ErrorMessage = "The amount is required.")]
        public decimal Amount { get; set; }

        // Fecha en la que se registró el gasto (obligatoria)
        [Required(ErrorMessage = "The date is required.")]
        public DateTime Date { get; set; }

        // Clave foránea que conecta el gasto con un grupo (obligatoria)
        [Required(ErrorMessage = "The group ID is required.")]
        public int GroupId { get; set; }

        // Propiedad de navegación: referencia al grupo asociado al gasto
        public Group Group { get; set; }
    }
}
