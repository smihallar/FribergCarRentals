using FribergCarRentals.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FribergCarRentals.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Car> Cars { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
       
    }
}
