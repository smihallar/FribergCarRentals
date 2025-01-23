using System.ComponentModel.DataAnnotations;

namespace FribergCarRentals.ViewModels
{
    public class LoginRegisterViewModel
    {
        public LoginViewModel Login { get; set; } = new LoginViewModel();

        public RegisterViewModel Register { get; set; } = new RegisterViewModel();

    }
}
