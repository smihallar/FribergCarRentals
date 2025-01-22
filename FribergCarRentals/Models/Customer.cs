using FribergCarRentals.Data;

namespace FribergCarRentals.Models
{
    public class Customer : SystemUser
    {
        public List<Booking> Bookings { get; set; }
    }
}
