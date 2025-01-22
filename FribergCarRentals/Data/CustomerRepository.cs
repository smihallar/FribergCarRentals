using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public CustomerRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IEnumerable<Customer> GetAll()
        {
            return applicationDbContext.Customers.OrderBy(c=>c.LastName);
        }

        public Customer GetById(int id)
        {
            return applicationDbContext.Customers.FirstOrDefault(c => c.Id == id);
        }
    }
}
