    using Microsoft.AspNetCore.Mvc;
    using RentACar.Models;
    using RentACar.Services;

    namespace RentACar.Controllers
    {
        public class UsersController:Controller
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

                await userService.UpdateAsync(user);
                return RedirectToAction(nameof(Index));
            }
        }
    }
