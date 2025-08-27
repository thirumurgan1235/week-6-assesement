using Microsoft.EntityFrameworkCore;

namespace DeptApi.Models
{
    public class DeptDbContext : DbContext
    {
        public DeptDbContext(DbContextOptions<DeptDbContext> options) : base(options) { }

        public DbSet<Dept> Departments { get; set; }
       public DbSet<Manager> Managers { get; set; }
    }
}
