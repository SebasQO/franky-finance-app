namespace FrankyFinance.Models
{
    public class DashboardViewModel
    {
        public int TotalGroups { get; set; }
        public int TotalExpenses { get; set; }
        public List<(int Id, string Name)> Groups { get; set; } = new List<(int, string)>();
    }
}
