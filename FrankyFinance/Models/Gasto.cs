using System.ComponentModel.DataAnnotations;

namespace FrankyFinance.Models
{
    public class Gasto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "The description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The amount is required.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "The date is required.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "The group ID is required.")]
        public int GroupId { get; set; }
        public Group Group { get; set; }

    }
}
