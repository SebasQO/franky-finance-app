using FrankyFinance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrankyFinance.Controllers
{
    public class GastosController : Controller
    {
        private readonly AppDbContext _context;

        public GastosController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult CrearGasto(int groupId)
        {
            var group = _context.Grupos.Include(g => g.GroupUsers).ThenInclude(gu => gu.User).FirstOrDefault(g => g.Id == groupId);
            if (group == null) return NotFound();

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

            ViewBag.Users = group.GroupUsers.Select(gu => gu.User).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult CrearGasto(ExpenseSplitViewModel model)
        {
            if (ModelState.IsValid)
            {
                var gasto = new Gasto
                {
                    Description = model.Description,
                    Amount = model.Amount,
                    Date = DateTime.Now,
                    GroupId = model.GroupId
                };

                _context.Gastos.Add(gasto);
                _context.SaveChanges();

                foreach (var division in model.Divisions)
                {
                    if (model.SelectedUserIds.Contains(division.UserId))
                    {
                        var split = new ExpenseSplit
                        {
                            GastoId = gasto.Id,
                            UserId = division.UserId,
                            Amount = division.Amount
                        };
                        _context.ExpenseSplits.Add(split);
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("Detalles", "Grupos", new { id = model.GroupId });
            }

            ViewBag.Users = _context.Users.ToList();
            return View(model);
        }




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

        [HttpGet]
        public IActionResult EditarGasto(int id)
        {
            var gasto = _context.Gastos.FirstOrDefault(g => g.Id == id);

            if (gasto == null)
            {
                return NotFound();
            }

            return View(gasto);
        }
        [HttpPost]
        public IActionResult EditarGasto(Gasto gasto)
        {
            if (ModelState.IsValid)
            {
                var existingGasto = _context.Gastos.FirstOrDefault(g => g.Id == gasto.Id);
                if (existingGasto != null)
                {
                    existingGasto.Description = gasto.Description;
                    existingGasto.Amount = gasto.Amount;
                    existingGasto.Date = DateTime.Now; // Fecha de última modificación

                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Expense updated successfully!";
                    return RedirectToAction("Detalles", "Grupos", new { id = existingGasto.GroupId });
                }

                TempData["ErrorMessage"] = "Expense not found.";
            }
            return View(gasto);
        }


    }
}
