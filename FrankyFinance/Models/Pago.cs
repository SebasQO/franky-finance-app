using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FrankyFinance.Models
{
    // Modelo que representa un pago entre dos usuarios dentro de un grupo
    public class Pago
    {
        public int Id { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public int PagadorId { get; set; }
        public User Pagador { get; set; }

        public int ReceptorId { get; set; }
        public User Receptor { get; set; }

        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
    }
}
