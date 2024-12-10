using System.ComponentModel.DataAnnotations;

namespace FrankyFinance.Models
{
    public class ExpenseSplitViewModel
    {
        public int GroupId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public int PaidByUserId { get; set; } // Quién pagó el gasto

        public List<int> SelectedUserIds { get; set; } = new List<int>(); // Usuarios seleccionados

        [Required]
        public string DivisionMethod { get; set; } // Iguales, Monto exacto, Porcentaje

        public List<ExpenseDivision> Divisions { get; set; } = new List<ExpenseDivision>();
    }
    public class ExpenseDivision
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal Amount { get; set; }
        public decimal Percentage { get; set; }
    }
}
