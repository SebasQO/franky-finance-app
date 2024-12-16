namespace FrankyFinance.Models
{
    // ViewModel que representa un resumen de deudas entre usuarios
    public class ResumenDeudasViewModel
    {
        // Nombre del usuario que debe dinero (deudor)
        public string Deudor { get; set; }

        // Nombre del usuario al que se le debe el dinero (acreedor)
        public string Acreedor { get; set; }

        // Monto total de la deuda
        public decimal Monto { get; set; }
    }
}
