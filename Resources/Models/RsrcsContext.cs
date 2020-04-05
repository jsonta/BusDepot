using Microsoft.EntityFrameworkCore;

namespace BusDepot.Models
{
    public class RsrcsContext : DbContext
    {
        public RsrcsContext(DbContextOptions<RsrcsContext> options) : base(options)
        {
        }

        public DbSet<Bus> Buses { get; set; }
        public DbSet<Driver> Drivers { get; set; }
    }
}
