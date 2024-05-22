using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using projetGestionProduit.Models;
using System.Security.Claims;

namespace projetGestionProduit.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var username = User.Identity.Name;

            var users = await _context.Users.ToListAsync();
            return View(users);
        }

        // GET: /User/Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: /User/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                var userModel = new User
                {
                    Username = user.Username,
                    Email = user.Email,
                    Password = PasswordHasher.HashPassword(user.Password)

                };

                _context.Add(userModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Login");
            }
            return View(user);
        }

        // GET: /User/Login
        public IActionResult Login()
        {
            return View();
        }
        // POST: /User/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(User login)
        {
            if (!ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
                if (user != null && VerifyPassword(login.Password, user.Password))
                {

                    // L'utilisateur est authentifié

                    // L'utilisateur est authentifié
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                
            };

                    var userIdentity = new ClaimsIdentity(claims, "login");

                    ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                    await HttpContext.SignInAsync(principal);


                    return RedirectToAction("Index");
                }
                else
                {
                    // Échec de l'authentification, afficher un message d'erreur
                    ModelState.AddModelError(string.Empty, "Nom d'utilisateur ou mot de passe incorrect.");
                }
            }
            return View(login); // Redirect to the login view with errors
        }

        private bool VerifyPassword(string enteredPassword, string hashedPassword)
        {
            // Compare le hachage du mot de passe entré avec le hachage du mot de passe stocké dans la base de données
            return PasswordHasher.HashPassword(enteredPassword) == hashedPassword;
        }

        // GET: /User/Logout
        public IActionResult Logout()
        {
            // Sign out the user and clear the authentication cookie
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect the user to the home page or the login page
            return RedirectToAction("Index", "Home"); // You can change the redirection target as needed
        }


    }
}
