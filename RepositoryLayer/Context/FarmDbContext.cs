using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Entity;
namespace RepositoryLayer.Context{
     public class FarmDbContext : DbContext
    {
        public FarmDbContext(DbContextOptions<FarmDbContext> options) : base(options)
        {
        }

        // Ye tumhara Users table represent karega
        public DbSet<UsersEntity> Users { get; set; }

        // Future tables:
        // public DbSet<CropEntity> Crops { get; set; }
        // public DbSet<BidEntity> Bids { get; set; }
        // public DbSet<InsuranceEntity> Insurances { get; set; }

        // (Optional) Table names customize karne ke liye:
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsersEntity>().ToTable("Users");
            // modelBuilder.Entity<CropEntity>().ToTable("Crops");
        }
    }
}
