using FribergCarRentals.Models;
using Microsoft.AspNetCore.Http.HttpResults;

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

        public void Delete(Booking booking)
        {  
                applicationDbContext.Bookings.Remove(booking);
                applicationDbContext.SaveChanges();
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
