using Microsoft.AspNetCore.Mvc;
using RentACar.Services;

namespace RentACar.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService authService;

        public AuthController()
        {
            authService = new AuthService();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var isValid = authService.ValidateLogin(username, password);

            if (!isValid)
            {
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.ErrorMessage = "Username and password are required.";
                return View();
            }

            ViewBag.SuccessMessage = "Registration successful.";
            return View();
        }

        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}