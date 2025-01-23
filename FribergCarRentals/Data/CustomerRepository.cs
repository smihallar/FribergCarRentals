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
             return applicationDbContext.Customers.FirstOrDefault(c => c.Email == email);
            
               
            
        }

        public Customer GetById(int id)
        {
           return applicationDbContext.Customers.FirstOrDefault(c => c.Id == id);
           
                
            
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
