using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public class CarRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CarRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public IEnumerable<Car> GetAll()
        {
            return applicationDbContext.Cars.OrderBy(c => c.PricePerDay);
        }

        public Car GetById(int id)
        {
            return applicationDbContext.Cars.FirstOrDefault(c => c.Id == id);
        }

        public void Add(Car car)
        {
            applicationDbContext.Cars.Add(car);
            applicationDbContext.SaveChanges();
        }

        public void Delete(Car car)
        {
            applicationDbContext.Remove(car);
            applicationDbContext.SaveChanges();
        }

        public void Update(Car car)
        {
            applicationDbContext.Cars.Update(car);
            applicationDbContext.SaveChanges();
        }
    }
}
