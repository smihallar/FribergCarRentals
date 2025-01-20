using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public interface IUserRepository
    {
        User GetById(int id);
        IEnumerable<User> GetAll();
    }
}
