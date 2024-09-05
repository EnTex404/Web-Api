using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Db
{
    public class MSSQLContext : DbContext
    {

        public MSSQLContext(DbContextOptions<MSSQLContext> options) : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Cabinet> Cabinets { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Area> Areas { get; set; }

    }
}
