using Microsoft.EntityFrameworkCore;

namespace E_CommerceWebApp.Services
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
             new Product { ProductID = 1, ProductName = "Fancy Product", Description = "Fancy", Price = 40 },
             new Product { ProductID = 2, ProductName = "Special Item", Description = "Special", Price = 80 },
             new Product { ProductID = 3, ProductName = "Sale Item", Description = "Sale", Price = 20 },
             new Product { ProductID = 4, ProductName = "Popular Item", Description = "Popular", Price = 100 }
            );
        }
    }
}
