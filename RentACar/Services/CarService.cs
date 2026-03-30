using RentACar.Models;

namespace RentACar.Services
{
    public class CarService
    {
        private static readonly List<Car> cars = new List<Car>();

        public List<Car> GetAll()
        {
            return cars;
        }

        public Car? GetById(int id)
        {
            return cars.FirstOrDefault(c => c.Id == id);
        }

        public void Add(Car car)
        {
            car.Id = cars.Count == 0 ? 1 : cars.Max(c => c.Id) + 1;
            cars.Add(car);
        }

        public void Update(Car updatedCar)
        {
            var car = GetById(updatedCar.Id);

            if (car == null)
            {
                return;
            }

            car.Brand = updatedCar.Brand;
            car.Model = updatedCar.Model;
            car.Year = updatedCar.Year;
            car.Seats = updatedCar.Seats;
            car.Description = updatedCar.Description;
            car.PricePerDay = updatedCar.PricePerDay;
        }

        public void Delete(int id)
        {
            var car = GetById(id);

            if (car != null)
            {
                cars.Remove(car);
            }
        }
    }
}