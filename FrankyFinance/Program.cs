using FrankyFinance.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDistributedMemoryCache(); // Requerido para manejar la sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Configuración de tiempo de vida de la sesión
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();


// Configurar DbContext
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar autenticación
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.Cookie.SameSite = SameSiteMode.Lax; // Compatibilidad con navegadores modernos
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
})
.AddGoogle(googleOptions =>
{
    googleOptions.ClientId = "ID";
    googleOptions.ClientSecret = "Scret";
    googleOptions.CallbackPath = "/signin-google"; 

    googleOptions.Events.OnCreatingTicket = async context =>
    {
        var dbContext = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();

        // Obtener la información del usuario autenticado desde Google
        var email = context.Principal.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        var name = context.Principal.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

        if (!string.IsNullOrEmpty(email))
        {
            // Verificar si el usuario ya existe en la base de datos
            var user = await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                // Si el usuario no existe, lo creamos
                user = new User
                {
                    Name = name ?? "Google User",
                    Email = email,
                    Password = ""
                };

                dbContext.Users.Add(user);
                await dbContext.SaveChangesAsync();
            }

            // Guardar el nombre del usuario en la sesión
            context.HttpContext.Session.SetString("UserName", user.Name);
        }
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession(); // Habilitar el middleware de sesión
app.UseAuthentication(); // Habilitar la autenticación
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
