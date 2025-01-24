using FribergCarRentals.Models;

namespace FribergCarRentals.Data
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext applicationDbContext;

        public AdminRepository(ApplicationDbContext applicationDbContext)
        {
            this.applicationDbContext = applicationDbContext;
        }
        public IEnumerable<Admin> GetAll()
        {
            return applicationDbContext.Admins.OrderBy(a => a.LastName); 
        }

        public Admin GetByEmail(string email)
        {
            return applicationDbContext.Admins.FirstOrDefault(a => a.Email == email);
        }

        
    }
}
