using System.ComponentModel.DataAnnotations;

namespace FrankyFinance.Models
{
    public class Expense
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public int GroupId { get; set; }
        public Group Group { get; set; }
    }
}
