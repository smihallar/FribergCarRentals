using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public interface ICarRepository
    {
        Car GetById(int id);
        IEnumerable<Car> GetAll();
        void Add(Car car);
        void Update(Car car);
        void Delete(int id);
    }
}
