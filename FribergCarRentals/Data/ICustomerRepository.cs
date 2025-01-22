using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public interface ICustomerRepository
    {
        Customer GetById(int id);
        Customer GetByEmail(string email);
        void Add(Customer customer);
        void Delete(Customer customer);
        void Update(Customer customer);
        void AddBooking(int customerId, Booking booking);
        IEnumerable<Customer> GetAll();
    }
}
