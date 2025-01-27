using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public BookingRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public void Add(Booking booking)
        {
            applicationDbContext.Bookings.Add(booking);
            applicationDbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            var booking = applicationDbContext.Bookings.Find(id);
            if (booking != null)
            {
                applicationDbContext.Bookings.Remove(booking);
                applicationDbContext.SaveChanges();
            }
        }

        public IEnumerable<Booking> GetAll()
        {
            return applicationDbContext.Bookings.ToList();
        }

        public Booking GetById(int id)
        {
            return applicationDbContext.Bookings.Find(id);
        }

        public void Update(Booking booking)
        {
            applicationDbContext.Update(booking);
            applicationDbContext.SaveChanges();
        }
    }
}
