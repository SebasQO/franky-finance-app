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

        // Muestra el formulario para crear un grupo
        [HttpGet]
        public IActionResult CrearGrupo()
        {
            var userId = HttpContext.Session.GetInt32("UserId"); // Obtén el UserId de la sesión
            ViewBag.UserId = userId;
            return View();
        }

        // Procesa la creación de un nuevo grupo y asigna al creador como "Owner"
        [HttpPost]
        public IActionResult CrearGrupo(Group group)
        {
            if (ModelState.IsValid)
            {
                // Guardar el grupo
                _context.Grupos.Add(group);
                _context.SaveChanges();

                // Obtener el UserId de la sesión
                var userId = HttpContext.Session.GetInt32("UserId");

                if (userId == null || !_context.Users.Any(u => u.Id == userId.Value))
                {
                    Console.WriteLine("UserId is invalid or does not exist.");
                    return RedirectToAction("Login", "Account");
                }

                // Asignar el usuario actual como administrador del grupo
                var groupUser = new GroupUser
                {
                    GroupId = group.Id,
                    UserId = userId.Value,
                    Role = "Owner"
                };

                _context.GroupUsers.Add(groupUser);
                _context.SaveChanges();

                return RedirectToAction("Dashboard", "Account");
            }

            return View(group);
        }




        // Muestra los detalles de un grupo específico
        [HttpGet]
        public IActionResult Detalles(int id)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");

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

            // Verificar si el usuario actual es Owner
            ViewBag.IsOwner = _context.GroupUsers
                .Any(gu => gu.GroupId == id && gu.UserId == currentUserId && gu.Role == "Owner");

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

        // Elimina un miembro del grupo (solo Owner puede realizarlo)
        [HttpPost]
        public IActionResult EliminarMiembro(int groupId, int userId)
        {
            var currentUserId = HttpContext.Session.GetInt32("UserId");

            // Verificar si el usuario actual es Owner
            var isOwner = _context.GroupUsers
                .Any(gu => gu.GroupId == groupId && gu.UserId == currentUserId && gu.Role == "Owner");

            if (!isOwner)
            {
                TempData["ErrorMessage"] = "You do not have permission to remove members.";
                return RedirectToAction("Detalles", new { id = groupId });
            }

            // Eliminar el miembro
            var member = _context.GroupUsers.FirstOrDefault(gu => gu.GroupId == groupId && gu.UserId == userId);
            if (member != null)
            {
                _context.GroupUsers.Remove(member);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Member removed successfully!";
            }

            return RedirectToAction("Detalles", new { id = groupId });
        }

        // Elimina un grupo si no tiene gastos asociados
        [HttpPost]
        public IActionResult EliminarGrupo(int id)
        {
            var grupo = _context.Grupos
                .Include(g => g.Gastos) // Verifica si hay gastos relacionados
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
        public IActionResult EditarGrupo(int id)
        {
            // Obtener el ID del usuario actual
            var userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
            {
                TempData["ErrorMessage"] = "You must be logged in.";
                return RedirectToAction("Login", "Account");
            }

            // Verificar si el usuario es administrador del grupo
            var isAdmin = _context.GroupUsers.Any(gu => gu.GroupId == id && gu.UserId == userId && gu.Role == "Admin");

            if (!isAdmin)
            {
                TempData["ErrorMessage"] = "You do not have permission to edit this group.";
                return RedirectToAction("Detalles", new { id });
            }

            // Obtener el grupo
            var group = _context.Grupos.Find(id);
            if (group == null) return NotFound();

            return View(group);
        }

        [HttpPost]
        public IActionResult EditarGrupo(Group model)
        {
            if (ModelState.IsValid)
            {
                _context.Grupos.Update(model);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Group updated successfully!";
                return RedirectToAction("Detalles", new { id = model.Id });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult AgregarUsuario(int groupId)
        {
            var group = _context.Grupos.FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound();
            }

            var viewModel = new GroupUserViewModel
            {
                GroupId = groupId, // Asigna el ID del grupo aquí
                Users = _context.Users.ToList() // Usuarios disponibles
            };

            return View(viewModel);
        }



        [HttpPost]
        public IActionResult AgregarUsuario(int groupId, int userId)
        {
            // Validar que el grupo existe
            var group = _context.Grupos.FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                return NotFound("The group does not exist.");
            }

            // Validar que el usuario existe
            var user = _context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return NotFound("The user does not exist.");
            }

            // Verificar si el usuario ya pertenece al grupo
            var existingGroupUser = _context.GroupUsers
                .FirstOrDefault(gu => gu.GroupId == groupId && gu.UserId == userId);

            if (existingGroupUser != null)
            {
                ModelState.AddModelError("", "The user is already in the group.");
                return RedirectToAction("Detalles", "Grupos", new { id = groupId });
            }

            // Agregar el usuario al grupo
            var groupUser = new GroupUser
            {
                GroupId = groupId,
                UserId = userId,
                Role = "Member" // Asigna el rol por defecto
            };

            _context.GroupUsers.Add(groupUser);
            _context.SaveChanges();

            // Redirigir a la vista de detalles
            TempData["SuccessMessage"] = "User added successfully!";
            return RedirectToAction("Detalles", "Grupos", new { id = groupId });
        }


    }
}
