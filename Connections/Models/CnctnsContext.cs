using Microsoft.EntityFrameworkCore;

namespace Connections.Models
{
    public class CnctnsContext : DbContext
    {
        public CnctnsContext(DbContextOptions<CnctnsContext> options) : base(options)
        {
        }

        public DbSet<Brigade> brigades { get; set; }
        public DbSet<Timetable> brigades_timetable { get; set; }
        public DbSet<Line> lines { get; set; }
        public DbSet<Relation> relations { get; set; }
        public DbSet<Remark> remarks { get; set; }
        public DbSet<Terminus> terminus { get; set; }
    }
}
