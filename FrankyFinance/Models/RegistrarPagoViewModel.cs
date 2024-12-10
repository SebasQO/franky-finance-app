using System.ComponentModel.DataAnnotations;

namespace FrankyFinance.Models
{
    public class RegistrarPagoViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }

        [Required(ErrorMessage = "Please select the payer.")]
        public int PagadorId { get; set; }

        [Required(ErrorMessage = "Please select the receiver.")]
        public int ReceptorId { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        public decimal Amount { get; set; }

        public List<UserViewModel> Users { get; set; } = new List<UserViewModel>();
    }
    public class UserViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
