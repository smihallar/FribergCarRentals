using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public class CarRepository: ICarRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CarRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IEnumerable<Car> GetAll()
        {
            var cars = applicationDbContext.Cars.OrderBy(c => c.PricePerDay).ToList();
            foreach (var car in cars)
            {
                car.ImageLinks = EnsureImageLinksFormat(car.ImageLinks);
            }
            return cars;
        }

        public Car GetById(int id)
        {
            var car = applicationDbContext.Cars.FirstOrDefault(c => c.Id == id);
            if (car != null)
            {
                car.ImageLinks = EnsureImageLinksFormat(car.ImageLinks);
            }
            return car;
        }

        public void Add(Car car)
        {
            car.ImageLinks = EnsureImageLinksFormat(car.ImageLinks);
            applicationDbContext.Cars.Add(car);
            applicationDbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var car = applicationDbContext.Cars.Find(id);
            if (car != null)
            {
                applicationDbContext.Cars.Remove(car);
                applicationDbContext.SaveChanges();
            }
        }

        public void Update(Car car)
        {
            car.ImageLinks = EnsureImageLinksFormat(car.ImageLinks);
            applicationDbContext.Cars.Update(car);
            applicationDbContext.SaveChanges();
        }

        private List<string> EnsureImageLinksFormat(List<string> imageLinks)
        {
            if (imageLinks == null || !imageLinks.Any())
            {
                return new List<string>();
            }

            // Ensure each link is a valid URL (optional)
            for (int i = 0; i < imageLinks.Count; i++)
            {
                if (!Uri.IsWellFormedUriString(imageLinks[i], UriKind.Absolute))
                {
                    imageLinks[i] = string.Empty;
                }
            }

            return imageLinks;
        }
    }

}

