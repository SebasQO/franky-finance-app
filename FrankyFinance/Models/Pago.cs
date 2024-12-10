using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrankyFinance.Models
{
    public class Pago
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int GroupId { get; set; }
        public Group Group { get; set; }

        [Required]
        public int PagadorId { get; set; }

        [ForeignKey("PagadorId")]
        public User Pagador { get; set; }

        [Required]
        public int ReceptorId { get; set; }

        [ForeignKey("ReceptorId")]
        public User Receptor { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
