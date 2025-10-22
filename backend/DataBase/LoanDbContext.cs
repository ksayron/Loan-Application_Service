using Microsoft.EntityFrameworkCore;
using TriInkom.Entities;

namespace TriInkom.DataBase
{
    public class LoanDbContext : DbContext
    {
        public LoanDbContext(DbContextOptions<LoanDbContext> options) : base(options) {  }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>().ToTable("Applications").HasKey(app => app.Number);
        }
    }
}
