using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.ViewModels
{
    public class CarViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public decimal PricePerDay { get; set; }

        [Required]
        public List<string> ImageLinks { get; set; }

        public bool IsAvailable { get; set; }
    }
}
