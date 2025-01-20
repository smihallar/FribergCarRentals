using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public UserRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }

        public IEnumerable<User> GetAll()
        {
            return applicationDbContext.Users.OrderBy(u=>u.LastName);
        }

        public User GetById(int id)
        {
            return applicationDbContext.Users.FirstOrDefault(u => u.Id == id);
        }
    }
}
