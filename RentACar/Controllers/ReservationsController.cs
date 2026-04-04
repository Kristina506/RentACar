using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentACar.Models;
using RentACar.Services;
using System.Security.Claims;

namespace RentACar.Controllers
{
    [Authorize]
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
            if (User.IsInRole("Admin"))
            {
                var allReservations = await reservationService.GetAllAsync();
                return View(allReservations);
            }

            var userId = GetCurrentUserId();

            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var userReservations = await reservationService.GetByUserIdAsync(userId.Value);
            return View(userReservations);
        }

        public async Task<IActionResult> Details(int id)
        {
            var reservation = await reservationService.GetByIdAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            if (!User.IsInRole("Admin"))
            {
                var userId = GetCurrentUserId();

                if (userId == null)
                {
                    return RedirectToAction("Login", "Auth");
                }

                if (reservation.UserId != userId.Value)
                {
                    return Forbid();
                }
            }

            return View(reservation);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Users = await userService.GetAllAsync();
            ViewBag.Cars = await carService.GetAllAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            if (reservation.EndDate.Date < reservation.StartDate.Date)
            {
                ModelState.AddModelError("", "End date cannot be earlier than start date.");
            }

            bool isAvailable = await reservationService.IsCarAvailableAsync(
                reservation.CarId,
                reservation.StartDate,
                reservation.EndDate);

            if (!isAvailable)
            {
                ModelState.AddModelError("", "The car is already reserved for this period.");
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

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, Reservation reservation)
        {
            if (id != reservation.Id)
            {
                return NotFound();
            }

            if (reservation.EndDate.Date < reservation.StartDate.Date)
            {
                ModelState.AddModelError("", "End date cannot be earlier than start date.");
            }

            bool isAvailable = await reservationService.IsCarAvailableAsync(
                reservation.CarId,
                reservation.StartDate,
                reservation.EndDate,
                reservation.Id);

            if (!isAvailable)
            {
                ModelState.AddModelError("", "The car is already reserved for this period.");
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

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await reservationService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                              ?? User.FindFirst("UserId")?.Value;

            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }

            return null;
        }
    }
}