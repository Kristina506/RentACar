using RentACar.Data;
using RentACar.Models;
using Microsoft.EntityFrameworkCore;

namespace RentACar.Services
{
    public class ReservationService
    {
        private readonly ApplicationDbContext context;

        public ReservationService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Reservation>> GetAllAsync()
        {
            return await context.Reservations
                .Include(r => r.User)
                .Include(r => r.Car)
                .ToListAsync();
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await context.Reservations
                .Include(r => r.User)
                .Include(r => r.Car)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> IsCarAvailableAsync(int carId, DateTime startDate, DateTime endDate, int? reservationId = null)
        {
            return !await context.Reservations.AnyAsync(r =>
                r.CarId == carId &&
                (reservationId == null || r.Id != reservationId) &&
                startDate <= r.EndDate &&
                endDate >= r.StartDate);
        }

        public async Task AddAsync(Reservation reservation)
        {
            var car = await context.Cars.FirstOrDefaultAsync(c => c.Id == reservation.CarId);

            if (car != null)
            {
                var days = (reservation.EndDate - reservation.StartDate).Days;
                if (days <= 0)
                {
                    days = 1;
                }

                reservation.TotalPrice = days * car.PricePerDay;
            }

            context.Reservations.Add(reservation);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            var existingReservation = await context.Reservations.FindAsync(reservation.Id);

            if (existingReservation != null)
            {
                existingReservation.UserId = reservation.UserId;
                existingReservation.CarId = reservation.CarId;
                existingReservation.StartDate = reservation.StartDate;
                existingReservation.EndDate = reservation.EndDate;

                var car = await context.Cars.FirstOrDefaultAsync(c => c.Id == reservation.CarId);

                if (car != null)
                {
                    var days = (reservation.EndDate - reservation.StartDate).Days;
                    if (days <= 0)
                    {
                        days = 1;
                    }

                    existingReservation.TotalPrice = days * car.PricePerDay;
                }

                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var reservation = await context.Reservations.FindAsync(id);

            if (reservation != null)
            {
                context.Reservations.Remove(reservation);
                await context.SaveChangesAsync();
            }
        }
    }
}