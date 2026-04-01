using Microsoft.AspNetCore.Mvc;
using RentACar.Models;
using RentACar.Services;

namespace RentACar.Controllers
{
    public class ReservationsController:Controller
    {
        private readonly ReservationService reservationService;
        private readonly UserService userService;
        private readonly CarService carService;

        public ReservationsController(
            ReservationService reservationService,
            UserService userService,
            CarService carService)
        {
            this.reservationService = reservationService;
            this.userService = userService;
            this.carService = carService;
        }

        public async Task<IActionResult> Index()
        {
            var reservations = await reservationService.GetAllAsync();
            return View(reservations);
        }

        public async Task<IActionResult> Details(int id)
        {
            var reservation = await reservationService.GetByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Users = await userService.GetAllAsync();
            ViewBag.Cars = await carService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if (reservation.EndDate < reservation.StartDate)
            {
                ModelState.AddModelError("", "End date cannot be before start date.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Users = await userService.GetAllAsync();
                ViewBag.Cars = await carService.GetAllAsync();
                return View(reservation);
            }

            await reservationService.AddAsync(reservation);
            return RedirectToAction(nameof(Index));
        }
    }
}
