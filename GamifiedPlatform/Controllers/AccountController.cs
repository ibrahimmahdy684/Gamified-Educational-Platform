using GamifiedPlatform.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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

                if (model.Type == "Learner")
                {
                    var learner = new Learner
                    {
                        UserId = user.UserId,
                        FirstName = model.Name,
                        Email = model.Email
                    };
                    _context.Learners.Add(learner);
                }
                else if (model.Type == "Instructor")
                {
                    var instructor = new Instructor
                    {
                        UserId = user.UserId,
                        Name = model.Name,
                        Email = model.Email // Or you can just reference model.Email from Users table
                    };
                    _context.Instructors.Add(instructor);
                }
                else if (model.Type == "Admin")
                {
                    var admin = new Admin
                    {
                        UserId = user.UserId,
                        Name = model.Name,
                        Email = model.Email
                    };
                    _context.Admins.Add(admin);
                }

                // Save the changes for the corresponding table
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

                // Redirect based on the user type
                if (user.Type == "Learner")
                {
                    return RedirectToAction("Profile", "Learners", new { id = user.UserId });
                }
                else if (user.Type == "Instructor")
                {
                    return RedirectToAction("Profile", "Instructors", new { id = user.UserId });
                }
                else if (user.Type == "Admin")
                {
                    return RedirectToAction("Profile", "Admins", new { id = user.UserId });
                }

                // If none of the conditions match, you can handle it accordingly.
                return RedirectToAction("Index", "Home");
            }

            return View(model);
        }

    }
}
