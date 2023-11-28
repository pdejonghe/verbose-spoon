using Microsoft.EntityFrameworkCore;
using ProceedixDemoApp.Models;

namespace ProceedixDemoApp.Data
{
    public class ProceedixDemoAppDbContext : DbContext
    {
        public ProceedixDemoAppDbContext(DbContextOptions<ProceedixDemoAppDbContext> options) : base(options)
        {
        }

        public DbSet<Application> Applications { get; set; }
        public DbSet<LogMessage> LogMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<Models.LogLevel>();
            modelBuilder.ApplyConfiguration(new LogMessageConfiguration());
            modelBuilder.ApplyConfiguration(new ApplicationConfiguration());
        }
    }
}