using FrankyFinance.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FrankyFinance.Controllers
{
    // Controlador para gestionar la cuenta de usuario: inicio de sesión, registro y autenticación externa
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;

        // Constructor que inicializa el contexto de base de datos
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // Muestra la vista de inicio de sesión
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Procesa la información del formulario de inicio de sesión
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Busca el usuario por correo electrónico
                var user = _context.Users.FirstOrDefault(u => u.Email == model.Email);

                // Valida si el usuario existe y la contraseña coincide
                if (user == null || user.Password != model.Password)
                {
                    TempData["ErrorMessage"] = "Invalid email or password.";
                    return View(model);
                }

                // Guarda el nombre del usuario en la sesión
                HttpContext.Session.SetString("UserName", user.Name);
                TempData["SuccessMessage"] = "Welcome back, " + user.Name + "!";
                HttpContext.Session.SetInt32("UserID", user.Id);
                return RedirectToAction("Dashboard");
            }

            TempData["ErrorMessage"] = "Invalid input. Please check your details.";
            return View(model);
        }

        // Muestra la vista de registro de usuario
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // Procesa el formulario de registro y guarda un nuevo usuario en la base de datos
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var user = new User
                    {
                        Name = model.Name,
                        Email = model.Email,
                        Password = model.Password
                    };

                    _context.Users.Add(user);
                    _context.SaveChanges();

                    ViewBag.RegistroExitoso = true;

                    TempData["SuccessMessage"] = "Account created successfully!";
                    return RedirectToAction("Login");
                }
                catch
                {
                    TempData["ErrorMessage"] = "An error occurred while creating the account.";
                }
            }
            else
            {
                TempData["ErrorMessage"] = "Please correct the errors in the form.";
            }

            return View(model);
        }

        // Muestra el dashboard con estadísticas e información del usuario
        [HttpGet]
        public IActionResult Dashboard()
        {
            var userName = HttpContext.Session.GetString("UserName") ?? "Guest";

            // Obtiene los datos para estadísticas
            var groups = _context.Grupos.ToList();
            var totalGastos = _context.Gastos.Sum(g => g.Amount);
            var totalPagos = _context.Pagos.Sum(p => p.Amount);

            var model = new DashboardViewModel
            {
                TotalGroups = groups.Count,
                TotalGastos = totalGastos,
                TotalPagos = totalPagos,
                Groups = groups.Select(g => (g.Id, g.Name)).ToList()
            };

            ViewBag.UserName = userName;
            return View(model);
        }

        // Muestra la vista para crear un nuevo grupo
        [HttpGet]
        public IActionResult CrearGrupo()
        {
            return View();
        }

        // Procesa el formulario para crear un grupo nuevo
        [HttpPost]
        public IActionResult CrearGrupo(Group group)
        {
            if (ModelState.IsValid)
            {
                // Guarda el grupo principal
                _context.Grupos.Add(group);
                _context.SaveChanges();

                // Asigna el usuario actual como "Owner"
                var userId = HttpContext.Session.GetInt32("UserId"); // Asegúrate de que UserId esté almacenado en la sesión
                if (userId != null)
                {
                    var groupUser = new GroupUser
                    {
                        GroupId = group.Id,
                        UserId = userId.Value,
                        Role = "Owner"
                    };
                    _context.GroupUsers.Add(groupUser); // Guarda la relación entre usuario y grupo
                    _context.SaveChanges();
                }

                return RedirectToAction("Dashboard", "Account");
            }

            return View(group);
        }

        // Inicia la autenticación de usuario con Google
        [HttpGet]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = "/Account/GoogleResponse" };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        // Procesa la respuesta de autenticación de Google
        [HttpGet]
        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            if (result?.Principal != null)
            {
                var claims = result.Principal.Identities.FirstOrDefault()?.Claims;                

                // Extrae el nombre y el correo del usuario autenticado
                var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var user = _context.Users.FirstOrDefault(u => u.Email == email);

                // Guarda el nombre en la sesión
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", name ?? "Guest");
                TempData["SuccessMessage"] = "Logged in successfully with Google!";

                return RedirectToAction("Dashboard", "Account");
            }

            TempData["ErrorMessage"] = "Failed to login with Google.";
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Cierra la sesión de autenticación
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Limpia la sesión de la aplicación
            HttpContext.Session.Clear();

            // Redirige a la página de inicio de sesión
            return RedirectToAction("Login", "Account");
        }
    }
}
