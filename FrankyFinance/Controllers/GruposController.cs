using FrankyFinance.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FrankyFinance.Controllers
{
    // Controlador para la gestión de grupos: creación, eliminación, detalles y gestión de usuarios
    public class GruposController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor que inicializa el contexto de base de datos
        public GruposController(AppDbContext context)
        {
            _context = context;
        }

        // Muestra el formulario para crear un grupo
        [HttpGet]
        public IActionResult CrearGrupo()
        {
            return View();
        }

        // Procesa la creación de un nuevo grupo
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

        // Muestra los detalles de un grupo específico
        [HttpGet]
        public IActionResult Detalles(int id)
        {
            // Obtiene el grupo con todos sus datos relacionados (gastos, pagos y usuarios)
            var group = _context.Grupos
                .Include(g => g.Gastos)
                .Include(g => g.Pagos)
                    .ThenInclude(p => p.Pagador)
                .Include(g => g.Pagos)
                    .ThenInclude(p => p.Receptor)
                .Include(g => g.GroupUsers)
                    .ThenInclude(gu => gu.User)
                .FirstOrDefault(g => g.Id == id);

            if (group == null) return NotFound();

            // Genera un resumen de gastos por fecha
            var resumenGastos = group.Gastos
                .GroupBy(g => g.Date.Date)
                .Select(g => new
                {
                    Fecha = g.Key,
                    Total = g.Sum(x => x.Amount)
                })
                .OrderBy(r => r.Fecha)
                .ToList();

            // Genera un resumen de pagos entre usuarios
            var resumenPagos = group.Pagos
                .GroupBy(p => new { PagadorName = p.Pagador.Name, ReceptorName = p.Receptor.Name })
                .Select(p => new
                {
                    Pagador = p.Key.PagadorName,
                    Receptor = p.Key.ReceptorName,
                    Total = p.Sum(x => x.Amount)
                })
                .ToList();

            // Calcula saldos pendientes de cada usuario
            var saldos = new Dictionary<string, decimal>();

            foreach (var member in group.GroupUsers.Select(gu => gu.User))
            {
                saldos[member.Name] = 0;
            }

            // Ajusta los saldos según gastos y pagos
            foreach (var gasto in group.Gastos)
            {
                saldos[gasto.Group.GroupUsers.FirstOrDefault(u => u.User.Id == gasto.GroupId)?.User.Name ?? "Unknown"] += gasto.Amount;
            }

            foreach (var pago in group.Pagos)
            {
                saldos[pago.Pagador.Name] -= pago.Amount;
                saldos[pago.Receptor.Name] += pago.Amount;
            }

            // Calcula las deudas entre usuarios
            var deudas = new List<ResumenDeudasViewModel>();

            foreach (var acreedor in saldos.Where(s => s.Value > 0))
            {
                foreach (var deudor in saldos.Where(s => s.Value < 0))
                {
                    var monto = Math.Min(acreedor.Value, Math.Abs(deudor.Value));
                    if (monto > 0)
                    {
                        deudas.Add(new ResumenDeudasViewModel
                        {
                            Deudor = deudor.Key,
                            Acreedor = acreedor.Key,
                            Monto = monto
                        });

                        saldos[acreedor.Key] -= monto;
                        saldos[deudor.Key] += monto;
                    }
                }
            }

            // Construye el modelo para la vista
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
                Pagos = group.Pagos.ToList(),
                ResumenDeudas = deudas
            };

            return View(model);
        }

        // Elimina un grupo si no tiene gastos asociados
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

        // Muestra el formulario para agregar un usuario a un grupo
        [HttpGet]
        public IActionResult AgregarUsuario(int groupId)
        {
            var group = _context.Grupos.FirstOrDefault(g => g.Id == groupId);
            if (group == null) return NotFound();

            ViewBag.GroupId = groupId;
            ViewBag.Users = _context.Users.ToList();
            return View();
        }

        // Procesa la adición de un usuario a un grupo
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
