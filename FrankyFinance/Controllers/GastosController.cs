using FrankyFinance.Models;
using Microsoft.AspNetCore.Mvc;

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
            var group = _context.Grupos.FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }

            ViewBag.GroupName = group.Name;
            return View(new Gasto { GroupId = groupId });
        }

        [HttpPost]
        public IActionResult CrearGasto(Gasto gasto)
        {
            Console.WriteLine("POST request received");

            if (ModelState.IsValid)
            {
                Console.WriteLine($"Saving expense: GroupId={gasto.GroupId}, Description={gasto.Description}, Amount={gasto.Amount}");
                gasto.Date = DateTime.Now; // Registrar la fecha actual
                _context.Gastos.Add(gasto);
                _context.SaveChanges();

                Console.WriteLine("Expense saved successfully!");
                return RedirectToAction("Detalles", "Grupos", new { id = gasto.GroupId });
            }

            Console.WriteLine("Model is invalid");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Validation Error: {error.ErrorMessage}");
            }

            var group = _context.Grupos.FirstOrDefault(g => g.Id == gasto.GroupId);
            ViewBag.GroupName = group?.Name;
            return View(gasto);
        }

    }
}
