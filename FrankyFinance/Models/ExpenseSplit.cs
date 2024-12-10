namespace FrankyFinance.Models
{
    public class ExpenseSplit
    {
        public int Id { get; set; }
        public int GastoId { get; set; }
        public Gasto Gasto { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public decimal Amount { get; set; }
    }
}
