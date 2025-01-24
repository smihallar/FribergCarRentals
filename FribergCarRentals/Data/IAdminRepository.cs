using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public interface IAdminRepository
    {
        Admin GetByEmail(string email);
        IEnumerable<Admin> GetAll();
    }
}
