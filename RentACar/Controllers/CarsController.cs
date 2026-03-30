using Microsoft.AspNetCore.Mvc;
using RentACar.Models;
using RentACar.Services;

namespace RentACar.Controllers
{
    public class CarsController : Controller
    {
        private readonly CarService carService;

        public CarsController()
        {
            carService = new CarService();
        }

        public IActionResult Index()
        {
            var cars = carService.GetAll();
            return View(cars);
        }

        public IActionResult Details(int id)
        {
            var car = carService.GetById(id);

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
        public IActionResult Create(Car car)
        {
            if (!ModelState.IsValid)
            {
                return View(car);
            }

            carService.Add(car);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var car = carService.GetById(id);

            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        [HttpPost]
        public IActionResult Edit(Car car)
        {
            if (!ModelState.IsValid)
            {
                return View(car);
            }

            carService.Update(car);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            carService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}