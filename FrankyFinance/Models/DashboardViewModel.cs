namespace FrankyFinance.Models
{
    // Modelo para la vista del Dashboard, que muestra estadísticas generales y datos recientes
    public class DashboardViewModel
    {
        // Total de grupos existentes
        public int TotalGroups { get; set; }

        // Suma total de todos los gastos
        public decimal TotalGastos { get; set; }

        // Suma total de todos los pagos
        public decimal TotalPagos { get; set; }

        // Total de transacciones de gastos registradas
        public int TotalExpenses { get; set; }

        // Lista de grupos con sus IDs y nombres
        public List<(int Id, string Name)> Groups { get; set; } = new List<(int, string)>();

        // Lista de los gastos recientes para mostrar en el dashboard
        public List<ExpenseViewModel> RecentExpenses { get; set; } = new List<ExpenseViewModel>();
    }
}
