using FrankyFinance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrankyFinance.Controllers
{
    public class GruposController : Controller
    {
        private readonly AppDbContext _context;

        public GruposController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult CrearGrupo()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CrearGrupo(Group group)
        {
            if (ModelState.IsValid)
            {
                _context.Grupos.Add(group);
                _context.SaveChanges();

                return RedirectToAction("Dashboard", "Account");
            }

            return View(group);
        }

        [HttpGet]
        public IActionResult Detalles(int id)
        {
            var group = _context.Grupos
                .Include(g => g.Gastos)
                .Include(g => g.Pagos)
                    .ThenInclude(p => p.Pagador)
                .Include(g => g.Pagos)
                    .ThenInclude(p => p.Receptor)
                .Include(g => g.GroupUsers)
                    .ThenInclude(gu => gu.User)
                .FirstOrDefault(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            var resumenGastos = group.Gastos
                .GroupBy(g => g.Date.Date)
                .Select(g => new
                {
                    Fecha = g.Key,
                    Total = g.Sum(x => x.Amount)
                })
                .OrderBy(r => r.Fecha)
                .ToList();

            var model = new ResumenGastosViewModel
            {
                Id = group.Id,
                GroupName = group.Name,
                Description = group.Description,
                TotalGastos = group.Gastos.Sum(g => g.Amount),


                ResumenPorFecha = resumenGastos.Select(r => new ResumenFecha
                {
                    Fecha = r.Fecha,
                    Total = r.Total
                }).ToList(),

                Gastos = group.Gastos.ToList(),

                GroupUsers = group.GroupUsers.ToList(),

                Pagos = group.Pagos.ToList()
            };

            return View(model);
        }



        [HttpPost]
        public IActionResult EliminarGrupo(int id)
        {
            var grupo = _context.Grupos
                .Include(g => g.Gastos)
                .FirstOrDefault(g => g.Id == id);

            if (grupo == null)
            {
                TempData["ErrorMessage"] = "Group not found.";
                return RedirectToAction("Dashboard", "Account");
            }

            if (grupo.Gastos.Any())
            {
                TempData["ErrorMessage"] = "Cannot delete group with associated expenses.";
                return RedirectToAction("Dashboard", "Account");
            }

            _context.Grupos.Remove(grupo);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Group deleted successfully!";
            return RedirectToAction("Dashboard", "Account");
        }

        [HttpGet]
        public IActionResult AgregarUsuario(int groupId)
        {
            var group = _context.Grupos.FirstOrDefault(g => g.Id == groupId);
            if (group == null) return NotFound();

            ViewBag.GroupId = groupId;
            ViewBag.Users = _context.Users.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AgregarUsuario(int groupId, int userId)
        {
            var groupUser = new GroupUser
            {
                GroupId = groupId,
                UserId = userId
            };

            _context.GroupUsers.Add(groupUser);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "User added to the group successfully!";
            return RedirectToAction("Detalles", new { id = groupId });
        }


    }
}
