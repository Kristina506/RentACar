using Microsoft.EntityFrameworkCore;
using RentACar.Data;
using RentACar.Models;

namespace RentACar.Services
{
    public class CarService
    {
        private readonly ApplicationDbContext context;

        public CarService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<Car>> GetAllAsync()
        {
            return await context.Cars.ToListAsync();
        }

        public async Task<Car?> GetByIdAsync(int id)
        {
            return await context.Cars.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task AddAsync(Car car)
        {
            context.Cars.Add(car);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Car updatedCar)
        {
            context.Cars.Update(updatedCar);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var car = await GetByIdAsync(id);

            if (car != null)
            {
                context.Cars.Remove(car);
                await context.SaveChangesAsync();
            }
        }
    }
}