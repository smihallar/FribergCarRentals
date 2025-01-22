namespace FribergCarRentals.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsCompleted { get; set; }
        public int CustomerId { get; set; }
    }
}
