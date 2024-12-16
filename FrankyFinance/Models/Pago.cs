using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrankyFinance.Models
{
    // Modelo que representa un pago entre dos usuarios dentro de un grupo
    public class Pago
    {
        // Identificador único del pago (clave primaria)
        [Key]
        public int Id { get; set; }

        // Clave foránea: Identificador del grupo al que pertenece el pago
        [Required]
        public int GroupId { get; set; }

        // Propiedad de navegación: Referencia al grupo asociado al pago
        public Group Group { get; set; }

        // Clave foránea: Identificador del usuario que realiza el pago (Pagador)
        [Required]
        public int PagadorId { get; set; }

        // Propiedad de navegación: Referencia al usuario que realizó el pago
        [ForeignKey("PagadorId")]
        public User Pagador { get; set; }

        // Clave foránea: Identificador del usuario que recibe el pago (Receptor)
        [Required]
        public int ReceptorId { get; set; }

        // Propiedad de navegación: Referencia al usuario que recibe el pago
        [ForeignKey("ReceptorId")]
        public User Receptor { get; set; }

        // Monto del pago realizado (obligatorio)
        [Required]
        public decimal Amount { get; set; }

        // Fecha en la que se registró el pago (obligatoria)
        [Required]
        public DateTime Date { get; set; }
    }
}
