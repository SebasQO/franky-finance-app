namespace FrankyFinance.Models
{
    // Modelo que representa la división de un gasto entre usuarios
    public class ExpenseSplit
    {
        // Identificador único de la división del gasto
        public int Id { get; set; }

        // Clave foránea que relaciona esta división con un gasto específico
        public int GastoId { get; set; }

        // Propiedad de navegación: referencia al gasto asociado
        public Gasto Gasto { get; set; }

        // Clave foránea que relaciona esta división con un usuario
        public int UserId { get; set; }

        // Propiedad de navegación: referencia al usuario asociado
        public User User { get; set; }

        // Monto que corresponde al usuario dentro de esta división del gasto
        public decimal Amount { get; set; }
    }
}
