using FribergCarRentals.Models;
using Microsoft.EntityFrameworkCore;

namespace FribergCarRentals.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CustomerRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public void AddBooking(int customerId, Booking booking)
        {
            var customer = applicationDbContext.Customers.Include(c => c.Bookings)
                                         .FirstOrDefault(c => c.Id == customerId);

            if (customer != null)
            {
                customer.Bookings.Add(booking);
                applicationDbContext.SaveChanges();
            }
        }

        public void Add(Customer customer)
        {
            applicationDbContext.Customers.Add(customer);
            applicationDbContext.SaveChanges();
        }

        public IEnumerable<Customer> GetAll()
        {
            return applicationDbContext.Customers.OrderBy(c=>c.LastName);
        }

        public Customer GetByEmail(string email)
        {
            var customer = applicationDbContext.Customers.FirstOrDefault(c => c.Email == email);
            if (customer != null)
            {
                return customer;
            }
        }

        public Customer GetById(int id)
        {
            var customer = applicationDbContext.Customers.FirstOrDefault(c => c.Id == id);
            if (customer != null)
            {
                return customer;
            }
        }

        public void Delete(Customer customer)
        {
            applicationDbContext.Remove(customer);
            applicationDbContext.SaveChanges();
        }

        public void Update(Customer customer)
        {
            applicationDbContext.Update(customer);
            applicationDbContext.SaveChanges();
        }
    }
}
