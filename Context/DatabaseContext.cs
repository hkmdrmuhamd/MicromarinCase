using Microsoft.EntityFrameworkCore;
using MicromarinCase.Entities;

namespace MicromarinCase.Context
{
    public class DatabaseContext : DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DatabaseContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=MUHAMMED;Database=case;Trusted_Connection=True;TrustServerCertificate=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Data>()
                .Property(d => d.JsonData)
                .HasColumnName("JsonData")
                .HasColumnType("nvarchar(max)");
        }

        public DbSet<Data> Datas { get; set; }
    }
}
