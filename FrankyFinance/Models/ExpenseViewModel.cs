namespace FrankyFinance.Models
{
    public class ExpenseViewModel
    {
        public int Id { get; set; }  // Agregar el Id
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public string Date { get; set; }
        public string GroupName { get; set; }
    }
}
