using Microsoft.EntityFrameworkCore;

namespace Resources.Models
{
    public class RsrcsContext : DbContext
    {
        public RsrcsContext(DbContextOptions<RsrcsContext> options) : base(options)
        {
        }

        public DbSet<Bus> buses { get; set; }
        public DbSet<Driver> drivers { get; set; }
    }
}
