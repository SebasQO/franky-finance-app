using FrankyFinance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrankyFinance.Controllers
{
    public class PagosController : Controller
    {
        private readonly AppDbContext _context;

        public PagosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult RegistrarPago(int groupId)
        {
            var group = _context.Grupos
                .Include(g => g.GroupUsers)
                    .ThenInclude(gu => gu.User)
                .FirstOrDefault(g => g.Id == groupId);

            if (group == null)
            {
                return NotFound();
            }

            // Asegúrate de inicializar correctamente el ViewModel
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

            return View(model); // Devuelve el modelo inicializado
        }


        [HttpPost]
        public IActionResult RegistrarPago(Pago pago)
        {
            if (ModelState.IsValid)
            {
                pago.Date = DateTime.Now;
                _context.Pagos.Add(pago);
                _context.SaveChanges();

                return RedirectToAction("Detalles", "Grupos", new { id = pago.GroupId });
            }

            ViewBag.Users = _context.Users.ToList();
            return View(pago);
        }
    }
}
