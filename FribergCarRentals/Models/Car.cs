using FribergCarRentals.Data;

namespace FribergCarRentals.Models
{
    public class Car
    {
        public int Id { get; set; }
        public List<string> ImageLinks { get; set; }
        public string Name { get; set; }
        public decimal PricePerDay { get; set; }
        public bool IsAvailable { get; set; }
    }
}
