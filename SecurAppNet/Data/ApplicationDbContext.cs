using Microsoft.EntityFrameworkCore;
using SecurAppNet.Models;

namespace SecurAppNet.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options) 
        { 
        }

        public DbSet<User> Users => Set<User>();  
    }
}