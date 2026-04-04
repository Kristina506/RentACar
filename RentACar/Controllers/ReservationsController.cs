using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Models;
using RentACar.Services;

namespace RentACar.Controllers
{
    [Authorize(Roles = "Admin,User")]
    public class ReservationsController : Controller
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

            bool isAvailable = await reservationService.IsCarAvailableAsync(
                reservation.CarId,
                reservation.StartDate,
                reservation.EndDate);

            if (!isAvailable)
            {
                ModelState.AddModelError("", "This car is already reserved for the selected period.");
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

        public async Task<IActionResult> Edit(int id)
        {
            var reservation = await reservationService.GetByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            ViewBag.Users = await userService.GetAllAsync();
            ViewBag.Cars = await carService.GetAllAsync();
            return View(reservation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Reservation reservation)
        {
            if (reservation.EndDate < reservation.StartDate)
            {
                ModelState.AddModelError("", "End date cannot be before start date.");
            }

            bool isAvailable = await reservationService.IsCarAvailableAsync(
                reservation.CarId,
                reservation.StartDate,
                reservation.EndDate,
                reservation.Id);

            if (!isAvailable)
            {
                ModelState.AddModelError("", "This car is already reserved for the selected period.");
            }

            if (!ModelState.IsValid)
            {
                ViewBag.Users = await userService.GetAllAsync();
                ViewBag.Cars = await carService.GetAllAsync();
                return View(reservation);
            }

            await reservationService.UpdateAsync(reservation);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var reservation = await reservationService.GetByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return View(reservation);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await reservationService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}