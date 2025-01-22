using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public interface ICustomerRepository
    {
        Customer GetById(int id);
        IEnumerable<Customer> GetAll();
    }
}
