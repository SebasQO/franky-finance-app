namespace FrankyFinance.Models
{
    // ViewModel para representar la información de un gasto en la vista
    public class ExpenseViewModel
    {
        // Identificador único del gasto
        public int Id { get; set; }

        // Descripción del gasto
        public string Description { get; set; }

        // Monto total del gasto
        public decimal Amount { get; set; }

        // Fecha del gasto en formato de texto
        public string Date { get; set; }

        // Nombre del grupo al que pertenece el gasto
        public string GroupName { get; set; }
    }
}
