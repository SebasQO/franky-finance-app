namespace FrankyFinance.Models
{
    // ViewModel que representa un resumen completo de los gastos de un grupo
    public class ResumenGastosViewModel
    {
        // Identificador del grupo
        public int Id { get; set; }

        // Nombre del grupo
        public string GroupName { get; set; }

        // Descripción del grupo
        public string Description { get; set; }

        // Total de todos los gastos registrados en el grupo
        public decimal TotalGastos { get; set; }

        // Lista de gastos agrupados por fecha con totales
        public List<ResumenFecha> ResumenPorFecha { get; set; }

        // Lista completa de gastos asociados al grupo
        public List<Gasto> Gastos { get; set; }

        // Lista de usuarios que pertenecen al grupo
        public List<GroupUser> GroupUsers { get; set; }

        // Lista de pagos realizados dentro del grupo
        public List<Pago> Pagos { get; set; } = new List<Pago>();

        // Lista de deudas pendientes entre usuarios del grupo
        public List<ResumenDeudasViewModel> ResumenDeudas { get; set; }
    }

    // Clase que representa un resumen de gastos por fecha
    public class ResumenFecha
    {
        // Fecha en la que se registraron los gastos
        public DateTime Fecha { get; set; }

        // Total de gastos realizados en la fecha especificada
        public decimal Total { get; set; }
    }
}
