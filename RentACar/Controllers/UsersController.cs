using Microsoft.AspNetCore.Mvc;
using RentACar.Models;
using RentACar.Services;
using Microsoft.AspNetCore.Authorization;

namespace RentACar.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private readonly UserService userService;

        public UsersController(UserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var users = await userService.GetAllAsync();
            return View(users);
        }

        public async Task<IActionResult> Details(int id)
        {
            var user = await userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            user.Password = AuthService.HashPassword(user.Password);

            await userService.AddAsync(user);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var user = await userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var existingUser = await userService.GetByIdAsync(user.Id);

            if (existingUser == null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(user.Password))
            {
                user.Password = existingUser.Password;
            }
            else
            {
                user.Password = AuthService.HashPassword(user.Password);
            }

            await userService.UpdateAsync(user);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id)
        {
            var user = await userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await userService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}