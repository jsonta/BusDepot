using Microsoft.EntityFrameworkCore;

namespace BusDepot.Models
{
    public class DepotContext : DbContext
    {
        public DepotContext(DbContextOptions<DepotContext> options) : base(options)
        {
        }

        public DbSet<Bus> Buses { get; set; }
        public DbSet<Driver> Drivers { get; set; }
    }
}
