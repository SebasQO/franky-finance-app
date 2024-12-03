using FrankyFinance.Models;
using Microsoft.AspNetCore.Mvc;

namespace FrankyFinance.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {             
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

                if (user == null || user.Password != model.Password)
                {
                    ModelState.AddModelError(string.Empty, "Invalid email or password.");
                    return View(model);
                }
                
                HttpContext.Session.SetString("UserName", user.Name);

                return RedirectToAction("Dashboard");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                
                var user = new User
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = model.Password 
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Account created successfully!";
                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Dashboard()
        {
            var userName = HttpContext.Session.GetString("UserName") ?? "Guest";

            var groups = _context.Grupos.ToList();
            var totalExpenses = _context.Gastos.Count();

            var model = new DashboardViewModel
            {
                TotalGroups = groups.Count,
                TotalExpenses = totalExpenses,
                Groups = groups.Select(g => (g.Id, g.Name)).ToList()
            };

            ViewBag.UserName = userName;
            return View(model);
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

                return RedirectToAction("Dashboard");
            }

            return View(model);
        }



    }
}
