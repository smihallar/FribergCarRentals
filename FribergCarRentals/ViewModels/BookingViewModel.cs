using FribergCarRentals.Models;
using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.ViewModels
{
    public class BookingViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; } = DateTime.Now;


        [Required]
        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(1);

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }
        public int SelectedCarId { get; set; }

        public List<Car> AvailableCars { get; set; } = new List<Car>();
        public decimal TotalCost { get; set; }


    }
}
