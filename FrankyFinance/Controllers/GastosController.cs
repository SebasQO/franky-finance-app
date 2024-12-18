using FrankyFinance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrankyFinance.Controllers
{
    // Controlador para gestionar los gastos: creación, eliminación y edición
    public class GastosController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor que inicializa el contexto de la base de datos
        public GastosController(AppDbContext context)
        {
            _context = context;
        }

        // Muestra el formulario para crear un gasto en un grupo específico
        [HttpGet]
        public IActionResult CrearGasto(int groupId)
        {
            // Busca el grupo y sus usuarios
            var group = _context.Grupos
                .Include(g => g.GroupUsers)
                .ThenInclude(gu => gu.User)
                .FirstOrDefault(g => g.Id == groupId);

            if (group == null) return NotFound();

            // Prepara el modelo con divisiones inicializadas en 0
            var model = new ExpenseSplitViewModel
            {
                GroupId = groupId,
                Divisions = group.GroupUsers.Select(gu => new ExpenseDivision
                {
                    UserId = gu.User.Id,
                    UserName = gu.User.Name,
                    Amount = 0
                }).ToList()
            };

            // Enviar lista de usuarios a la vista
            ViewBag.Users = group.GroupUsers.Select(gu => gu.User).ToList();
            return View(model);
        }

        // Procesa el formulario para crear un nuevo gasto
        [HttpPost]
        public IActionResult CrearGasto(ExpenseSplitViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Crea el gasto principal
                var gasto = new Gasto
                {
                    Description = model.Description,
                    Amount = model.Amount,
                    Date = DateTime.Now,
                    GroupId = model.GroupId
                };

                _context.Gastos.Add(gasto);
                _context.SaveChanges();

                //// Guarda las divisiones del gasto para los usuarios seleccionados
                //foreach (var division in model.Divisions)
                //{
                //    if (model.SelectedUserIds.Contains(division.UserId))
                //    {
                //        var split = new ExpenseSplit
                //        {
                //            GastoId = gasto.Id,
                //            UserId = division.UserId,
                //            Amount = division.Amount
                //        };
                //        _context.ExpenseSplits.Add(split);
                //    }
                //}

                _context.SaveChanges();
                return RedirectToAction("Detalles", "Grupos", new { id = model.GroupId });
            }

            // Si hay errores, se recarga la lista de usuarios
            ViewBag.Users = _context.Users.ToList();
            return View(model);
        }

        // Elimina un gasto por su ID
        [HttpPost]
        public IActionResult EliminarGasto(int id)
        {
            var gasto = _context.Gastos.FirstOrDefault(g => g.Id == id);

            if (gasto != null)
            {
                _context.Gastos.Remove(gasto);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Expense deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Expense not found.";
            }

            return RedirectToAction("Dashboard", "Account");
        }

        // Muestra el formulario para editar un gasto existente
        [HttpGet]
        public IActionResult EditarGasto(int id)
        {
            var gasto = _context.Gastos.FirstOrDefault(g => g.Id == id);

            if (gasto == null)
            {
                return NotFound();
            }

            var model = new Gasto
            {
                Id = gasto.Id,
                Description = gasto.Description,
                Amount = gasto.Amount,
                Date = gasto.Date,
                GroupId = gasto.GroupId
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult EditarGasto(Gasto model)
        {
            if (ModelState.IsValid)
            {
                var gasto = _context.Gastos.FirstOrDefault(g => g.Id == model.Id);

                if (gasto != null)
                {
                    gasto.Description = model.Description;
                    gasto.Amount = model.Amount;
                    gasto.Date = model.Date;

                    _context.SaveChanges();
                    return RedirectToAction("Detalles", "Grupos", new { id = gasto.GroupId });
                }
            }

            return View(model);
        }

    }
}
