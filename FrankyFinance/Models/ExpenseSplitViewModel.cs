using System.ComponentModel.DataAnnotations;

namespace FrankyFinance.Models
{
    // ViewModel para registrar y gestionar la división de un gasto
    public class ExpenseSplitViewModel
    {
        // Identificador del grupo al que pertenece el gasto
        public int GroupId { get; set; }

        // Descripción del gasto (obligatoria)
        [Required]
        public string Description { get; set; }

        // Monto total del gasto (obligatorio)
        [Required]
        public decimal Amount { get; set; }

        // ID del usuario que realizó el pago (obligatorio)
        [Required]
        public int PaidByUserId { get; set; } // Quién pagó el gasto

        // Lista de IDs de los usuarios seleccionados para dividir el gasto
        public List<int> SelectedUserIds { get; set; } = new List<int>();

        // Método de división del gasto (obligatorio)
        // Ejemplo: "Iguales", "Monto exacto", "Porcentaje"
        [Required]
        public string DivisionMethod { get; set; }

        // Lista de divisiones detalladas del gasto entre los usuarios
        public List<ExpenseDivision> Divisions { get; set; } = new List<ExpenseDivision>();
    }

    // Clase auxiliar que representa la división individual de un gasto
    public class ExpenseDivision
    {
        // ID del usuario al que se asigna una parte del gasto
        public int UserId { get; set; }

        // Nombre del usuario
        public string UserName { get; set; }

        // Monto específico asignado al usuario
        public decimal Amount { get; set; }

        // Porcentaje del gasto asignado al usuario (si la división es proporcional)
        public decimal Percentage { get; set; }
    }
}
