using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public interface IBookingRepository
    {
        Booking GetById(int id);
        IEnumerable<Booking> GetAll();
        void Add(Booking booking);
        void Update(Booking booking);
        void Delete(Booking booking);
    }
}
