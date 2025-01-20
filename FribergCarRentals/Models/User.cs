using FribergCarRentals.Data;

namespace FribergCarRentals.Models
{
    public class User : Account
    {
        public List<Booking> Bookings { get; set; }

    }
}
