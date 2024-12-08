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
        public IActionResult CrearGasto(Gasto gasto, int groupId)
        {
            Console.WriteLine("POST request received");

            
            if (gasto.GroupId == 0 || gasto.GroupId == null)
            {
                gasto.GroupId = groupId;
                Console.WriteLine($"Manually assigned GroupId: {groupId}");
            }

            Console.WriteLine($"GroupId: {gasto.GroupId}, Description: {gasto.Description}, Amount: {gasto.Amount}");

            if (ModelState.IsValid)
            {
                gasto.Date = DateTime.Now; 
                _context.Gastos.Add(gasto);
                _context.SaveChanges();

                Console.WriteLine("Expense saved successfully!");
                return RedirectToAction("Detalles", "Grupos", new { id = gasto.GroupId });
            }

            Console.WriteLine("ModelState is invalid");
            foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine($"Validation Error: {error.ErrorMessage}");
            }

            var group = _context.Grupos.FirstOrDefault(g => g.Id == gasto.GroupId);
            ViewBag.GroupName = group?.Name;
            return View(gasto);
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


    }
}
