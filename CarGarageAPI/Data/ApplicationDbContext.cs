using CarGarageAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CarGarageAPI.Data
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Car> Cars { get; set; }
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //seeding data
            modelBuilder.Entity<Car>().HasData(
                new Car { Vin = 1, BrandName = "Maruti Suzuki", ModelName = "Swift", Colour = "white", Price = 1000000},
                new Car { Vin = 2, BrandName = "Hyundai", ModelName = "i20", Colour = "silver", Price = 1200000 },
                new Car { Vin = 3, BrandName = "Maruti Suzuki", ModelName = "WagonR", Colour = "black", Price = 800000 },
                new Car { Vin = 4, BrandName = "Mahindra", ModelName = "Scorpio", Colour = "Grey", Price = 1400000 }
            );

        }
    }
}
