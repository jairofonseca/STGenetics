using Microsoft.EntityFrameworkCore;

namespace STGeneticsWebApp.Data
{
    public class STGeneticsDBContext : DbContext
    {
        public STGeneticsDBContext(DbContextOptions<STGeneticsDBContext> options) : base(options)
        {

        }

        public DbSet<Animal> Animals { get; set; }
        public DbSet<Breed> Breeds { get; set; }
        public DbSet<Sex> Sexes { get; set; }
        public DbSet<Status> Statuses { get; set; }
    }
}
