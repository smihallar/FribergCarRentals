using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.ViewModels
{
    public class LoginViewModel
    {

        [Required(ErrorMessage = "Email inte ifyllt korrekt")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lösenord inte ifyllt korrekt")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}
