using Microsoft.AspNetCore.Mvc;
using RentACar.Models;
using RentACar.Services;
using Microsoft.AspNetCore.Authorization;

namespace RentACar.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CarsController : Controller
    {
        private readonly CarService carService;

        public CarsController(CarService carService)
        {
            this.carService = carService;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await carService.GetAllAsync();
            return View(cars);
        }

        public async Task<IActionResult> Details(int id)
        {
            var car = await carService.GetByIdAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            if (!ModelState.IsValid)
            {
                return View(car);
            }

            await carService.AddAsync(car);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var car = await carService.GetByIdAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Car car)
        {
            if (!ModelState.IsValid)
            {
                return View(car);
            }

            await carService.UpdateAsync(car);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var car = await carService.GetByIdAsync(id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await carService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}