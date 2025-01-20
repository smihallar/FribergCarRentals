using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public interface IAdminRepository
    {
        Admin GetById(int id);
        IEnumerable<Admin> GetAll();
    }
}
