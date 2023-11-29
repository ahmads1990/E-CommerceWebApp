using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceWebApp.Services
{
    public class ApplicationDBContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        //public DbSet<ProductImage> ProductImages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().HasData(
             new Product { ProductImage = null, ProductID = 1, ProductName = "Fancy Product", Description = "Fancy", Price = 40 },
             new Product { ProductImage = null, ProductID = 2, ProductName = "Special Item", Description = "Special", Price = 80 },
             new Product { ProductImage = null, ProductID = 3, ProductName = "Sale Item", Description = "Sale", Price = 20 },
             new Product { ProductImage = null, ProductID = 4, ProductName = "Popular Item", Description = "Popular", Price = 100 }
            );
            // Configure the ImageData property to be stored as binary data
            //modelBuilder.Entity<ProductImage>()
            //    .Property(e => e.ImageData)
            //    .HasColumnType("varbinary(max)");
        }
    }
}
