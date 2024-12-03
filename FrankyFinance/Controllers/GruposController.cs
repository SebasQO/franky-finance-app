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
        public IActionResult CrearGrupo(Group model)
        {
            if (ModelState.IsValid)
            {
                _context.Grupos.Add(model);
                _context.SaveChanges();

                return RedirectToAction("Dashboard", "Account");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Detalles(int id)
        {
            var group = _context.Grupos
                .Include(g => g.Gastos) 
                .FirstOrDefault(g => g.Id == id);

            if (group == null)
            {
                return NotFound();
            }

            return View(group);
        }


    }
}
