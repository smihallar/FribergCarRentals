using FribergCarRentals.Models;

namespace FribergCarRentals.ViewModels
{
    public class CustomerBookingViewModel
    {
        public Customer Customer { get; set; }
        public Booking Booking { get; set; }
        public Car Car { get; set; }
    }
}
