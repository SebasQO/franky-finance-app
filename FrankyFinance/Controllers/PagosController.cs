using FrankyFinance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrankyFinance.Controllers
{
    // Controlador para la gestión de pagos dentro de los grupos
    public class PagosController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor que inicializa el contexto de base de datos
        public PagosController(AppDbContext context)
        {
            _context = context;
        }

        // Muestra el formulario para registrar un pago en un grupo específico
        [HttpGet]
        public IActionResult RegistrarPago(int groupId)
        {
            // Busca el grupo junto con sus usuarios
            var group = _context.Grupos
                .Include(g => g.GroupUsers)
                    .ThenInclude(gu => gu.User)
                .FirstOrDefault(g => g.Id == groupId);

            // Verifica si el grupo existe
            if (group == null)
            {
                return NotFound();
            }

            // Inicializa el ViewModel con los datos del grupo y sus usuarios
            var model = new RegistrarPagoViewModel
            {
                GroupId = group.Id,
                GroupName = group.Name,
                Users = group.GroupUsers.Select(gu => new UserViewModel
                {
                    UserId = gu.User.Id,
                    UserName = gu.User.Name
                }).ToList()
            };

            return View(model); // Devuelve el formulario con los datos inicializados
        }

        // Procesa la información del formulario para registrar un pago
        [HttpPost]
        public IActionResult RegistrarPago(Pago pago)
        {
            if (ModelState.IsValid)
            {
                // Asigna la fecha actual al pago
                pago.Date = DateTime.Now;

                // Guarda el pago en la base de datos
                _context.Pagos.Add(pago);
                _context.SaveChanges();

                // Redirige a la página de detalles del grupo después de registrar el pago
                return RedirectToAction("Detalles", "Grupos", new { id = pago.GroupId });
            }

            // Si hay errores, recarga la lista de usuarios y devuelve la vista
            ViewBag.Users = _context.Users.ToList();
            return View(pago);
        }
    }
}
