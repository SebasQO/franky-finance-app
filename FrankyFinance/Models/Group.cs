using System.ComponentModel.DataAnnotations;

namespace FrankyFinance.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Gasto> Gastos { get; set; } = new List<Gasto>();
        public ICollection<GroupUser> GroupUsers { get; set; } = new List<GroupUser>();
        public ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    }
}
