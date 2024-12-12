using GamifiedPlatform.Models;
using Microsoft.AspNetCore.Mvc;

namespace GamifiedPlatform.Controllers
{
    public class AccountController : Controller
    {
        private readonly GamifiedPlatformContext _context;

        public AccountController(GamifiedPlatformContext context)
        {
            _context = context;
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
                    Password = model.Password, // You should hash this in production
                    Type = model.Type
                };

                _context.Users.Add(user);
                _context.SaveChanges();

                return RedirectToAction("Login");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.Users
                    .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

                if (user == null)
                {
                    // If user is not found, show error message
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }

                // Redirect to user's profile page or dashboard if credentials are correct
                return RedirectToAction("Profile", "User", new { id = user.UserID });
            }

            return View(model);
        }
    }
}
    