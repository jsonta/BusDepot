using Microsoft.EntityFrameworkCore;

namespace Connections.Models
{
    public class CnctnsContext : DbContext
    {
        public CnctnsContext(DbContextOptions<CnctnsContext> options) : base(options)
        {
        }

        public DbSet<Brigade> Brigades { get; set; }
        public DbSet<Line> Lines { get; set; }
        public DbSet<Relation> Relations { get; set; }
        public DbSet<Remark> Remarks { get; set; }
        public DbSet<Terminus> Terminuss { get; set; }
    }
}
