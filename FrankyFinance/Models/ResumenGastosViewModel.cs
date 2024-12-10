namespace FrankyFinance.Models
{
    public class ResumenGastosViewModel
    {
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public decimal TotalGastos { get; set; }
        public List<ResumenFecha> ResumenPorFecha { get; set; }
        public List<Gasto> Gastos { get; set; }
        public List<GroupUser> GroupUsers { get; set; }
        public List<Pago> Pagos { get; set; } = new List<Pago>();
    }
    public class ResumenFecha
    {
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
    }
}
