using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.ViewModels
{
    public class LoginRegisterViewModel
    {
        public LoginViewModel Login { get; set; } = new LoginViewModel();

        public RegisterViewModel Register { get; set; } = new RegisterViewModel();

        public class LoginViewModel
        {

            [Required]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
        public class RegisterViewModel
        {
            [Required]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            [Required]
            [DataType(DataType.EmailAddress)]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }
    }
}
