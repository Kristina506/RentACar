using Microsoft.AspNetCore.Mvc;
using RentACar.Models;
using RentACar.Services;

namespace RentACar.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService authService;

        public AuthController(AuthService authService)
        {
            this.authService = authService;
        }

        public IActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(string username, string password)
        //{
        //    var user = await authService.LoginAsync(username, password);

        //    if (user == null)
        //    {
        //        ViewBag.ErrorMessage = "Invalid username or password.";
        //        return View();
        //    }

        //    // 👉 Записваме в Session
        //    HttpContext.Session.SetString("Username", user.Username);
        //    HttpContext.Session.SetString("Role", user.Role);

        //    TempData["SuccessMessage"] = $"Welcome, {user.FirstName}!";

        //    return RedirectToAction("Index", "Home");
        //}

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            //user.Role = "User";

            bool success = await authService.RegisterAsync(user);

            if (!success)
            {
                ViewBag.ErrorMessage = "Username, email or EGN already exists.";
                return View(user);
            }

            TempData["SuccessMessage"] = "Registration successful.";
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            TempData["SuccessMessage"] = "You have logged out.";
            return RedirectToAction("Index", "Home");
        }
    }
}