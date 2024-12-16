using System.ComponentModel.DataAnnotations;

namespace FrankyFinance.Models
{
    // ViewModel para manejar la información del formulario de registro de pagos
    public class RegistrarPagoViewModel
    {
        // Identificador del grupo al que pertenece el pago
        public int GroupId { get; set; }

        // Nombre del grupo al que pertenece el pago
        public string GroupName { get; set; }

        // Identificador del usuario que realiza el pago (obligatorio)
        [Required(ErrorMessage = "Please select the payer.")]
        public int PagadorId { get; set; }

        // Identificador del usuario que recibe el pago (obligatorio)
        [Required(ErrorMessage = "Please select the receiver.")]
        public int ReceptorId { get; set; }

        // Monto del pago (obligatorio y mayor que cero)
        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        // Lista de usuarios disponibles para seleccionar como pagador o receptor
        public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();
    }

    // ViewModel para representar la información básica de un usuario
    public class UserViewModel
    {
        // Identificador del usuario
        public int UserId { get; set; }

        // Nombre del usuario
        public string UserName { get; set; }
    }
}
