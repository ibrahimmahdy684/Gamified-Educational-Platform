using Microsoft.AspNetCore.Mvc;
using GamifiedPlatform.Models;

namespace GamifiedPlatform.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Add logic to save the user to the database
                // Redirect to another page upon success
                return RedirectToAction("Index", "Home");
            }

            return View(model); // Return the view with validation errors
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Add logic to authenticate the user
                // Redirect to another page upon success
                return RedirectToAction("Index", "Home");
            }

            return View(model); // Return the view with validation errors
        }
    }
}
